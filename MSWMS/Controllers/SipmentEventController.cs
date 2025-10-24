using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SipmentEventController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SipmentEventController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SipmentEvent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipmentEvent>>> GetShipmentEvents()
        {
            return await _context.ShipmentEvents.ToListAsync();
        }

        // GET: api/SipmentEvent/5
        [HttpGet("{id}")]
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

        // POST: api/SipmentEvent
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShipmentEvent>> PostShipmentEvent(ShipmentEvent shipmentEvent)
        {
            _context.ShipmentEvents.Add(shipmentEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShipmentEvent", new { id = shipmentEvent.Id }, shipmentEvent);
        }

        // DELETE: api/SipmentEvent/5
        [HttpDelete("{id}")]
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
