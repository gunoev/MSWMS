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
using MSWMS.Models.Responses;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetOrderItems(int orderId)
        {
            // speedup. now average 300 ms for 1000 items
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Id == orderId)
                .SelectMany(o => o.Items)
                .Include(inf => inf.ItemInfo.Take(1))
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    Barcode = i.ItemInfo.First().Barcode,
                    Variant = i.ItemInfo.First().Variant,
                    ItemNumber = i.ItemInfo.First().ItemNumber,
                    Description = i.ItemInfo.First().Description,
                    Price = i.ItemInfo.First().Price,
                    DiscountPrice = i.ItemInfo.First().DiscountPrice,
                    Quantity = (uint)i.NeededQuantity,
                    Scanned = (uint)_context.Scans.Count(s => s.Item.Id == i.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess)),
                    Remaining = i.NeededQuantity - _context.Scans.Count(s => s.Item.Id == i.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess))
                })
                .ToListAsync();
        }

        
        [HttpGet("remaining/{barcode}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetRemaining(string barcode)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Items.Any(i => 
                    i.ItemInfo.Any(info => info.Barcode == barcode) && 
                    i.NeededQuantity > _context.Scans.Count(s => s.Item.Id == i.Id && 
                                                                 (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess))))
                .Select(o => new
                {
                    Id = o.Id,
                    ShipmentId = o.ShipmentId,
                    TransferShipmentNumber = o.TransferShipmentNumber,
                    TransferOrderNumber = o.TransferOrderNumber,
                    CreatedAt = o.CreatedDateTime,
                    CollectedQuantity = o.Items
                        .Where(i => i.ItemInfo.Any(info => info.Barcode == barcode))
                        .Sum(i => _context.Scans.Count(s => s.Item.Id == i.Id && 
                                                           (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess))),
                    RemainingQuantity = o.Items
                        .Where(i => i.ItemInfo.Any(info => info.Barcode == barcode))
                        .Sum(i => i.NeededQuantity - _context.Scans.Count(s => s.Item.Id == i.Id && 
                                                                             (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess)))
                    
                })
                .ToListAsync();
        }

        // GET: api/Item/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            var startTime = DateTime.Now;
            var item = await _context.Items
                .AsNoTracking()
                .Include(i => i.ItemInfo.Take(1))
                .Where(i => i.Id == id)
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    Barcode = i.ItemInfo.First().Barcode,
                    Variant = i.ItemInfo.First().Variant,
                    ItemNumber = i.ItemInfo.First().ItemNumber,
                    Description = i.ItemInfo.First().Description,
                    Price = i.ItemInfo.First().Price,
                    DiscountPrice = i.ItemInfo.First().DiscountPrice,
                    Quantity = (uint)i.NeededQuantity,
                    Scanned = (uint)_context.Scans.Count(s => s.Item.Id == i.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess)),
                    Remaining = i.NeededQuantity - _context.Scans.Count(s => s.Item.Id == i.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess))
                }).FirstOrDefaultAsync();
                

            if (item == null)
            {
                return NotFound();
            }
            Console.WriteLine("GetItem endpoint time: " + (DateTime.Now - startTime));
            return item;
        }

        // PUT: api/Item/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = Policies.RequireAdmin)]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Item
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = Policies.RequireAdmin)]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Item/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.RequireAdmin)]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
