using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Hubs;
using MSWMS.Infrastructure.Authorization;
using MSWMS.Models.Requests;
using MSWMS.Models.Responses;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentEventController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ScanHub> _hubContext;

        public ShipmentEventController(AppDbContext context, IHubContext<ScanHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/SipmentEvent
        [HttpGet]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<IEnumerable<ShipmentEvent>>> GetShipmentEvents()
        {
            return await _context.ShipmentEvents.ToListAsync();
        }

        [HttpGet("Shipment/{shipmentId}")]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<IEnumerable<ShipmentEventDto>>> GetShipmentEventsByShipmentId(int shipmentId)
        {
            return await _context.ShipmentEvents
                .Where(e => e.Shipment.Id == shipmentId)
                .Select(e => new ShipmentEventDto
                {
                    Id = e.Id,
                    ShipmentId = e.Shipment.Id,
                    Timestamp = e.Timestamp,
                    Code = e.Code,
                    Location = e.Location.Name,
                    Box = e.Box != null ? new BoxDto 
                    { 
                        Id = e.Box.Id,
                        Guid = e.Box.Guid.ToString(),
                        BoxNumber = e.Box.BoxNumber,
                        UserId = e.Box.User.Id,
                        Username = e.Box.User.Username,
                        Quantity = _context.Scans
                            .Count(s => s.Box.Id == e.Box.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess))
                    } : null,
                    Order = e.Order != null ? new ShipmentOrderDto 
                    { 
                        Id = e.Order.Id,
                        ShipmentId = e.Shipment.Id,
                        DbCode = e.Order.ShipmentId,
                        TransferShipmentNumber = e.Order.TransferShipmentNumber,
                        TransferOrderNumber = e.Order.TransferOrderNumber,
                        Origin = e.Order.Origin.Name,
                        Destination = e.Order.Destination.Name,
                        Status = e.Order.Status.ToString(),
                    } : null,
                    User = e.User.Username,
                    Action = e.Action,
                    Status = e.Status
                })
                .OrderByDescending(s => s.Timestamp)
                .ToListAsync();
        }

        // GET: api/SipmentEvent/5
        [HttpGet("{id}")]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<ShipmentEvent>> GetShipmentEvent(int id)
        {
            var shipmentEvent = await _context.ShipmentEvents.FindAsync(id);

            if (shipmentEvent == null)
            {
                return NotFound();
            }

            return shipmentEvent;
        }

        // PUT: api/SipmentEvent/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = Policies.RequireAdmin)]
        public async Task<IActionResult> PutShipmentEvent(int id, ShipmentEvent shipmentEvent)
        {
            if (id != shipmentEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(shipmentEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentEventExists(id))
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

        [HttpPost("load")]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<ShipmentEvent>> PostLoadEvent(ShipmentEventRequest eventRequest)
        {
            if (eventRequest.Action != ShipmentEvent.ShipmentAction.Load)
            {
                return BadRequest("Action must be Load");
            }

            if (eventRequest.ShipmentId <= 0)
            {
                return BadRequest("ShipmentId must be set");
            }
            
            
            var user = _context.Users
                .Include(u => u.Location)
                .FirstOrDefault(u => u.Username == User.Identity.Name);
            
            if (user is null) return BadRequest();
            
            var location = user.Location;

            var shipment = await _context.Shipments
                .Include(s => s.Events)
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(s => s.Id == eventRequest.ShipmentId);

            if (shipment is null)
            {
                return NotFound("Shipment not found");
            }
            
            var orderIds = shipment.Orders.Select(o => o.Id).ToList();

            var box = await _context.Boxes
                .Include(b => b.Order)
                .ThenInclude(o => o.Origin)
                .Include(b => b.Order)
                .ThenInclude(o => o.Destination)
                .FirstOrDefaultAsync(b => b.Guid.ToString() == eventRequest.Code && orderIds.Contains(b.Order.Id)); 
            
            var eventStatus = ShipmentEvent.EventStatus.Ok;
            
            if (shipment.Events is null)
            {
                shipment.Events = new List<ShipmentEvent>();
            }
            
            if (box is null)
            {
                eventStatus = ShipmentEvent.EventStatus.NotFound;
            }
            else if (_context.ShipmentEvents.Any(e => e.Code == eventRequest.Code))
            {
                eventStatus = ShipmentEvent.EventStatus.Duplicate;
            }

            var shipmentEvent = new ShipmentEvent
            {
                Code = eventRequest.Code,
                Timestamp = DateTime.Now,
                Location = location,
                Box = box,
                User = user,
                Order = box?.Order?? null,
                Action = ShipmentEvent.ShipmentAction.Load,
                Status = eventStatus,
                Shipment = shipment,
            };
            
            shipment.Events.Add(shipmentEvent);
            _context.Shipments.Entry(shipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var eventDto = new ShipmentEventDto
            {
                Id = shipmentEvent.Id,
                ShipmentId = shipmentEvent.Shipment.Id,
                Timestamp = shipmentEvent.Timestamp,
                Code = shipmentEvent.Code,
                Location = shipmentEvent.Location.Name,
                Box = shipmentEvent.Box != null ? new BoxDto 
                { 
                    Id = shipmentEvent.Box.Id,
                    Guid = shipmentEvent.Box.Guid.ToString(),
                    BoxNumber = shipmentEvent.Box.BoxNumber,
                } : null,
                Order = shipmentEvent.Order != null ? new ShipmentOrderDto 
                { 
                    Id = shipmentEvent.Order.Id,
                    ShipmentId = shipmentEvent.Shipment.Id,
                    DbCode = shipmentEvent.Order.ShipmentId,
                    TransferShipmentNumber = shipmentEvent.Order.TransferShipmentNumber,
                    TransferOrderNumber = shipmentEvent.Order.TransferOrderNumber,
                    Origin = shipmentEvent.Order.Origin.Name,
                    Destination = shipmentEvent.Order.Destination.Name,
                    Status = shipmentEvent.Order.Status.ToString(),
                } : null,
                User = shipmentEvent.User.Username,
                Action = shipmentEvent.Action,
                Status = shipmentEvent.Status,
            };
            
            
            var groupName = $"Shipment_{shipment.Id}";
            await _hubContext.Clients.Group(groupName)
                .SendAsync("loadEventProcessed", eventDto);

            return Ok();
        }

        // POST: api/SipmentEvent
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<ShipmentEvent>> PostShipmentEvent(ShipmentEvent shipmentEvent)
        {
            _context.ShipmentEvents.Add(shipmentEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShipmentEvent", new { id = shipmentEvent.Id }, shipmentEvent);
        }

        // DELETE: api/SipmentEvent/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<IActionResult> DeleteShipmentEvent(int id)
        {
            var shipmentEvent = await _context.ShipmentEvents
                .Include(e => e.Shipment)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (shipmentEvent == null)
            {
                return NotFound();
            }

            _context.ShipmentEvents.Remove(shipmentEvent);
            await _context.SaveChangesAsync();
            
            var groupName = $"Shipment_{shipmentEvent.Shipment?.Id}";
            await _hubContext.Clients.Group(groupName)
                .SendAsync("loadEventDeleted", id);

            return NoContent();
        }

        private bool ShipmentEventExists(int id)
        {
            return _context.ShipmentEvents.Any(e => e.Id == id);
        }
    }
}
