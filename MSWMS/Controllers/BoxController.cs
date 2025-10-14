using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Models.Responses;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BoxController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<BoxDto>>> GetOrderBoxes(int orderId)
        {
            var boxes = await _context.Orders
                .AsNoTracking()
                .Where(o => o.Id == orderId)
                .SelectMany(o => o.Boxes)
                .Select(b => new BoxDto
                {
                    Id = b.Id,
                    Guid = b.Guid.ToString(),
                    BoxNumber = b.BoxNumber,
                    UserId = b.User.Id,
                    Username = b.User.Username,
                    Quantity = _context.Scans.
                        Count(s => s.Box.Id == b.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess))
                })
                .ToListAsync();

            return boxes;
        }

        // GET: api/Box/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoxDto>> GetBox(int id)
        {
            var box = await _context.Boxes
                .Select(b => new BoxDto
                {
                    Id = b.Id,
                    Guid = b.Guid.ToString(),
                    BoxNumber = b.BoxNumber,
                    UserId = b.User.Id,
                    Username = b.User.Username,
                    Quantity = _context.Scans
                        .Count(s => s.Box.Id == b.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess))
                })
                .FirstOrDefaultAsync(b => b.Id == id);

            if (box == null)
            {
                return NotFound();
            }

            return box;
        }

        // PUT: api/Box/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBox(int id, Box box)
        {
            if (id != box.Id)
            {
                return BadRequest();
            }

            _context.Entry(box).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoxExists(id))
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

        // POST: api/Box
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Box>> PostBox(Box box)
        {
            _context.Boxes.Add(box);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBox", new { id = box.Id }, box);
        }

        // DELETE: api/Box/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBox(int id)
        {
            var box = await _context.Boxes.FindAsync(id);
            if (box == null)
            {
                return NotFound();
            }

            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoxExists(int id)
        {
            return _context.Boxes.Any(e => e.Id == id);
        }
    }
}
