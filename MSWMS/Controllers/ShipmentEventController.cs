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

        [HttpPost]
        [Authorize(Policy = Policies.RequireLoadingOperator)]
        public async Task<ActionResult<ShipmentEvent>> PostLoadEvent(ShipmentEventRequest eventRequest)
        {
            if (eventRequest.Action != ShipmentEvent.ShipmentAction.Load)
            {
                BadRequest("Action must be Load");
            }

            if (eventRequest.ShipmentId <= 0)
            {
                BadRequest("ShipmentId must be set");
            }
            
            
            var user = _context.Users
                .Include(u => u.Location)
                .FirstOrDefault(u => u.Username == User.Identity.Name);
            
            if (user is null) return BadRequest();
            
            var location = user.Location;

            var shipment = await _context.Shipments.FindAsync(eventRequest.ShipmentId);

            var box = await _context.Boxes.FirstOrDefaultAsync(b => b.Guid.ToString() == eventRequest.Code);
            
            var eventStatus = ShipmentEvent.EventStatus.Ok;
            
            if (box is null)
            {
                eventStatus = ShipmentEvent.EventStatus.NotFound;
            }
            else if (shipment.Events.Any(e => e.Code == eventRequest.Code))
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
                Action = ShipmentEvent.ShipmentAction.Load,
                Status = eventStatus,
            };
            
            shipment.Events.Add(shipmentEvent);
            _context.Shipments.Entry(shipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            
            var groupName = $"Shipment_{shipment.Id}";
            await _hubContext.Clients.Group(groupName)
                .SendAsync("loadEventProcessed", shipmentEvent);

            return CreatedAtAction("GetShipmentEvent", new { id = shipmentEvent.Id }, shipmentEvent);
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
            var shipmentEvent = await _context.ShipmentEvents.FindAsync(id);
            if (shipmentEvent == null)
            {
                return NotFound();
            }

            _context.ShipmentEvents.Remove(shipmentEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShipmentEventExists(int id)
        {
            return _context.ShipmentEvents.Any(e => e.Id == id);
        }
    }
}
