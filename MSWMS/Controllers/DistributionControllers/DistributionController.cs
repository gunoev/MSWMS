using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Entities.Distributions;

namespace MSWMS.Controllers.DistributionControllers
{
    [Route("api/Distribution")]
    [ApiController]
    public class DistributionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DistributionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Distribution
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Distribution>>> GetDistributions()
        {
            return await _context.Distributions.ToListAsync();
        }

        // GET: api/Distribution/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DistributionDto>> GetDistribution(int id)
        {
            var distribution = await _context.Distributions.FindAsync(id);

            if (distribution == null)
            {
                return NotFound();
            }
            
            var distributionDto = new DistributionDto
            {
                Id = distribution.Id,
                Date = distribution.Date,
                Note = distribution.Note
            };

            return distributionDto;
        }

        // PUT: api/Distribution/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistribution(int id, Distribution distribution)
        {
            if (id != distribution.Id)
            {
                return BadRequest();
            }

            _context.Entry(distribution).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistributionExists(id))
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

        // POST: api/Distribution
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Distribution>> PostDistribution(CreateDistributionRequest request)
        {
            var distribution = new Distribution
            {
                Date = request.Date,
                Note = request.Note
            };

            _context.Distributions.Add(distribution);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistribution", new { id = distribution.Id }, distribution);
        }

        // DELETE: api/Distribution/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistribution(int id)
        {
            var distribution = await _context.Distributions.FindAsync(id);
            if (distribution == null)
            {
                return NotFound();
            }

            _context.Distributions.Remove(distribution);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DistributionExists(int id)
        {
            return _context.Distributions.Any(e => e.Id == id);
        }
    }
    
    public class CreateDistributionRequest
    {
        public required DateOnly Date { get; set; }
        public string Note { get; set; } = string.Empty;
    }

    public class DistributionDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
