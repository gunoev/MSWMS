using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Infrastructure.Helpers;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        // POST: api/Order/upload-excel
        [HttpPost("upload-excel")]
        public async Task<ActionResult<ExcelParsedOrder>> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var parser = new OrderExcelParser();
            var extension = Path.GetExtension(file.FileName);
            var tempFilePath = Path.GetTempFileName() + extension;
            
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var parsedOrder = await parser.Parse(tempFilePath);

            return parsedOrder;
        }

        // POST: api/Order/generate-test
        [HttpPost("generate-test")]
        public async Task<ActionResult<Order>> GenerateAndPostTestOrder()
        {
            _context.Locations.Add(new Location{Name = "Test Location"});
            _context.Locations.Add(new Location{Name = "Test Location2"});
            _context.SaveChanges();
            _context.Roles.Add(new Role
            {
                Name = "AdminTest",
                Type = Role.RoleType.Admin,
            });
            _context.SaveChanges();
            _context.Users.Add(new User
            {
                Username = "Test User",
                Status = Entities.User.UserStatus.Active,
                Location = _context.Locations.First(e => e.Name == "Test Location"),
                PasswordHash = "123",
                Roles = new List<Role>(),
            });
            await _context.SaveChangesAsync();
            var order = new Order
            {
                TransferOrderNumber = $"TO{DateTime.Now:yyyyMMddHHmmss}",
                TransferShipmentNumber = $"TS{DateTime.Now:yyyyMMddHHmmss}",
                Origin = _context.Locations.First(),
                Destination = _context.Locations.Skip(1).First(),
                ShipmentId = Guid.NewGuid().ToString(),
                Type = Order.OrderType.Distribution,
                CreatedBy = _context.Users.First(),
                Items = new List<Item>()
            };

            return await PostOrder(order);
        }
    }
}
