using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ShipmentController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Shipment
        
        [HttpGet]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<IEnumerable<ShipmentDto>>> GetShipments([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            if (from.HasValue && to.HasValue && (to.Value - from.Value).TotalDays > 7)
            {
                return BadRequest("Date range cannot be more than 1 week");
            }
            
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
            
            var shipmentIds = shipments.Select(s => s.Id).ToList();

            var orderIds = shipments.SelectMany(s => s.Orders.Select(o => o.Id)).ToList();

            var boxCounts = await _context.Boxes
                .Where(b => orderIds.Contains(b.Order.Id))
                .GroupBy(b => b.Order.Id)
                .Select(g => new { OrderId = g.Key, Count = g.Count() })
                .ToListAsync();

            var scanCounts = await _context.Scans
                .Where(s => orderIds.Contains(s.Order.Id))
                .GroupBy(s => s.Order.Id)
                .Select(g => new { OrderId = g.Key, Count = g.Count() })
                .ToListAsync();
            
            var boxCountDict = boxCounts.ToDictionary(x => x.OrderId, x => x.Count);
            
            var loadEvents = await _context.ShipmentEvents
                .Where(e => e.Order != null && e.Box != null &&
                            orderIds.Contains(e.Order.Id) &&
                            shipmentIds.Contains(e.Shipment.Id) &&
                            e.Action == ShipmentEvent.ShipmentAction.Load &&
                            e.Status == ShipmentEvent.EventStatus.Ok)
                .Select(e => new { ShipmentId = e.Shipment.Id, OrderId = e.Order.Id, BoxId = e.Box.Id })
                .ToListAsync();
            
            var loadedBoxIds = loadEvents.Select(e => e.BoxId).Distinct().ToList();

            var boxItemCounts = await _context.Scans
                .Where(s => s.Box != null && loadedBoxIds.Contains(s.Box.Id))
                .GroupBy(s => s.Box.Id)
                .Select(g => new { BoxId = g.Key, Count = g.Count() })
                .ToListAsync();
            
            var boxItemCountDict = boxItemCounts.ToDictionary(x => x.BoxId, x => x.Count);

            var shipmentOrderEvents = loadEvents
                .GroupBy(x => (x.ShipmentId, x.OrderId))
                .ToDictionary(g => g.Key, g => g.Select(x => x.BoxId).ToList());

            return shipments.Select(s => new ShipmentDto
            {
                Id = s.Id,
                Origin = s.Origin.Name,
                Destination = s.Destination.Name, 
                CreatedAt = s.CreatedAt,
                Scheduled = s.Scheduled,
                TotalBoxes = s.Orders.Sum(o => boxCountDict.GetValueOrDefault(o.Id, 0)),
                IsCompleted = false,
                Orders = s.Orders.Select(o => 
                {
                    var dto = _mapper.Map<ShipmentOrderDto>(o);

                    // Получаем список коробок конкретно для этой связки Shipment + Order
                    var loadedBoxesForThisShipment = shipmentOrderEvents.GetValueOrDefault((s.Id, o.Id), new List<int>());

                    dto.LoadedBoxes = loadedBoxesForThisShipment.Count;
                    dto.Boxes = boxCountDict.GetValueOrDefault(o.Id, 0);
                        
                    // Считаем items только для коробок, загруженных в ЭТОТ shipment
                    dto.TotalLoadedItems = loadedBoxesForThisShipment.Sum(boxId => boxItemCountDict.GetValueOrDefault(boxId, 0));
                        
                    return dto;
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

        [HttpGet("shipment-stats/{shipmentId}")]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<object>> GetShipmentStats(int shipmentId)
        {
            var orderIds = _context.Shipments.Where(s => s.Id == shipmentId).Select(s => s.Orders.Select(o => o.Id)).FirstOrDefault();
            var stat = new
            {
                TotalBoxes = _context.Shipments.Where(s => s.Id == shipmentId).Select(s => s.Orders.Sum(o => o.Boxes.Count)).FirstOrDefault(),
                TotalLoadedBoxes = await _context.ShipmentEvents.CountAsync(e => orderIds.Contains(e.Order.Id) && e.Action == ShipmentEvent.ShipmentAction.Load && e.Status == ShipmentEvent.EventStatus.Ok),
                LoadedBoxes = await _context.ShipmentEvents.CountAsync(e => e.Shipment.Id == shipmentId && e.Action == ShipmentEvent.ShipmentAction.Load && e.Status == ShipmentEvent.EventStatus.Ok),
            };
            
            return stat;
        }

        // PUT: api/Shipment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = Policies.RequireDispatcher)]
        public async Task<IActionResult> PutShipment(int id, CreateShipmentRequest request)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shipment == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(request.LocationId);
            if (location == null)
            {
                return NotFound("Location not found");
            }

            var orders = await _context.Orders.Where(o => request.OrderIds.Contains(o.Id)).ToListAsync();
            if (!orders.Any())
            {
                return NotFound("Orders not found");
            }

            shipment.Destination = location;
            shipment.Orders = orders;
            shipment.Scheduled = request.Scheduled;

            await _context.SaveChangesAsync();

            return Ok();
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
