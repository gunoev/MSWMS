using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemInfoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemInfoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ItemInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemInfo>>> GetItemInfos()
        {
            return await _context.ItemInfos.ToListAsync();
        }

        // GET: api/ItemInfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemInfo>> GetItemInfo(int id)
        {
            var itemInfo = await _context.ItemInfos.FindAsync(id);

            if (itemInfo == null)
            {
                return NotFound();
            }

            return itemInfo;
        }

        // PUT: api/ItemInfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemInfo(int id, ItemInfo itemInfo)
        {
            if (id != itemInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemInfoExists(id))
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

        // POST: api/ItemInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemInfo>> PostItemInfo(ItemInfo itemInfo)
        {
            _context.ItemInfos.Add(itemInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemInfo", new { id = itemInfo.Id }, itemInfo);
        }

        // DELETE: api/ItemInfo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemInfo(int id)
        {
            var itemInfo = await _context.ItemInfos.FindAsync(id);
            if (itemInfo == null)
            {
                return NotFound();
            }

            _context.ItemInfos.Remove(itemInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemInfoExists(int id)
        {
            return _context.ItemInfos.Any(e => e.Id == id);
        }
    }
}
