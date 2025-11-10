using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Hubs;
using MSWMS.Infrastructure.Authorization;
using MSWMS.Models.Responses;
using MSWMS.Services;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ScanHub> _hubContext;
        private readonly OrderService _orderService;

        public BoxController(AppDbContext context, IHubContext<ScanHub> hubContext, OrderService orderService)
        {
            _context = context;
            _hubContext = hubContext;
            _orderService = orderService;
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
        [Authorize(Policy = Policies.RequirePicker)]
        public async Task<IActionResult> DeleteBox(int id)
        {
            var box = await _context.Boxes
                .Include(b => b.User)
                .Include(b => b.Order)
                .FirstOrDefaultAsync(b => b.Id == id);
            
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
            
            if (box == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return BadRequest();
            }
            
            if (box.User != user && !user.Roles.Any(r => r.Type is Role.RoleType.Manager or Role.RoleType.Admin))
            {
                return BadRequest("You are not allowed to delete this box");
            }

            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();
            
            var groupName = $"Order_{box.Order.Id}";
            await _hubContext.Clients.Group(groupName).SendAsync("boxDeleted", box.Order.Id, box.Id, box.BoxNumber);
            
            await _orderService.UpdateOrderStatus(box.Order);


            return NoContent();
        }

        private bool BoxExists(int id)
        {
            return _context.Boxes.Any(e => e.Id == id);
        }
    }
}
