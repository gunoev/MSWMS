using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Infrastructure.Authorization;
using MSWMS.Models.Requests;
using MSWMS.Models.Responses;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShipmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Shipment
        
        [HttpGet]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<IEnumerable<ShipmentDto>>> GetShipments([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var query = _context.Shipments
                .Include(s => s.Origin)
                .Include(s => s.Destination)
                .Include(s => s.Orders)
                .ThenInclude(o => o.Items)
                .Include(s => s.Orders)
                    .ThenInclude(o => o.Origin)
                .Include(s => s.Orders)
                    .ThenInclude(o => o.Destination)
                .AsQueryable();
            
            if (from.HasValue)
                query = query.Where(s => s.Scheduled >= from.Value);
                
            if (to.HasValue)
                query = query.Where(s => s.Scheduled <= to.Value);

            var shipments = await query.ToListAsync();

            // Загружаем связанные данные отдельными запросами для лучшей производительности
            var shipmentIds = shipments.Select(s => s.Id).ToList();
            var orderIds = shipments.SelectMany(s => s.Orders.Select(o => o.Id)).ToList();

            // Загружаем количество коробок для каждого заказа
            var boxCounts = await _context.Boxes
                .Where(b => orderIds.Contains(b.Order.Id))
                .GroupBy(b => b.Order.Id)
                .Select(g => new { OrderId = g.Key, Count = g.Count() })
                .ToListAsync();

            // Загружаем общее количество товаров для каждого заказа
            var itemQuantities = 10;

            // Загружаем количество сканирований для каждого заказа
            var scanCounts = await _context.Scans
                .Where(s => orderIds.Contains(s.Order.Id))
                .GroupBy(s => s.Order.Id)
                .Select(g => new { OrderId = g.Key, Count = g.Count() })
                .ToListAsync();

            // Создаем словари для быстрого поиска
            var boxCountDict = boxCounts.ToDictionary(x => x.OrderId, x => x.Count);
            //var itemQuantityDict = itemQuantities.ToDictionary(x => x.OrderId, x => x.TotalQuantity);
            var scanCountDict = scanCounts.ToDictionary(x => x.OrderId, x => x.Count);

            return shipments.Select(s => new ShipmentDto
            {
                Id = s.Id,
                Origin = s.Origin.Name,
                Destination = s.Destination.Name, 
                CreatedAt = s.CreatedAt,
                Scheduled = s.Scheduled,
                TotalBoxes = s.Orders.Sum(o => boxCountDict.GetValueOrDefault(o.Id, 0)),
                IsCompleted = false,
                Orders = s.Orders.Select(o => new ShipmentOrderDto
                {
                    Id = o.Id,
                    ShipmentId = s.Id,
                    DbCode = o.ShipmentId,
                    TransferShipmentNumber = o.TransferShipmentNumber,
                    TransferOrderNumber = o.TransferOrderNumber,
                    Origin = o.Origin.Name,
                    Destination = o.Destination.Name,
                    Status = o.Status.ToString(),
                    TotalQuantity = o.Items.Sum(i => i.NeededQuantity),
                    TotalScanned = scanCountDict.GetValueOrDefault(o.Id, 0),
                    TotalRemaining = o.Items.Sum(i => i.NeededQuantity) - scanCountDict.GetValueOrDefault(o.Id, 0), // temporary value
                    Boxes = boxCountDict.GetValueOrDefault(o.Id, 0)
                }).ToList()
            }).ToList();
        }

        // GET: api/Shipment/5
        [HttpGet("{id}")]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<Shipment>> GetShipment(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);

            if (shipment == null)
            {
                return NotFound();
            }

            return shipment;
        }

        // PUT: api/Shipment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = Policies.RequireDispatcher)]
        public async Task<IActionResult> PutShipment(int id, Shipment shipment)
        {
            if (id != shipment.Id)
            {
                return BadRequest();
            }

            _context.Entry(shipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Shipment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = Policies.RequireDispatcher)]
        public async Task<ActionResult<Shipment>> PostShipment(CreateShipmentRequest shipment)
        {
            var location = await _context.Locations.FindAsync(shipment.LocationId);
            if (location == null)
            {
                return NotFound("Location not found");
            }
            var orders = await _context.Orders.Where(o => shipment.OrderIds.Contains(o.Id)).ToListAsync();
            if (!orders.Any())
            {
                return NotFound("Orders not found");
            }
            var user = await _context.Users
                .Include(u => u.Location)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            var entity = new Shipment
            {
                CreatedBy = user,
                Origin = user.Location,
                Destination = location,
                Orders = orders,
                CreatedAt = DateTime.Now,
                Scheduled = shipment.Scheduled
            };
            
            _context.Shipments.Add(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Shipment/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.RequireDispatcher)]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShipmentExists(int id)
        {
            return _context.Shipments.Any(e => e.Id == id);
        }
    }
}
