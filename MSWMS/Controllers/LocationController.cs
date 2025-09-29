using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Infrastructure.Authorization;

namespace MSWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly AppDbContext _context;

    public LocationController(AppDbContext context)
    {
        _context = context;
    }
    
    // GET: api/Location
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
    {
        return await _context.Locations.ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Location>> GetLocation(int id)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        return location;
    }
    
    // PUT: api/Location/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Policy = Policies.RequireManager)]
    public async Task<IActionResult> PutLocation(int id, Location location)
    {
        if (id != location.Id)
        {
            return BadRequest();
        }

        _context.Entry(location).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LocationExists(id))
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
    
    // POST: api/Location
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Policy = Policies.RequireManager)]
    public async Task<ActionResult<Location>> PostLocation(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLocation", new { id = location.Id }, location);
    }
    
    // DELETE: api/Location/5
    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.RequireManager)]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        if (id == 0)
        {
            return BadRequest("Cannot delete default location. Use update instead.");
        }
        
        var location = await _context.Locations.FindAsync(id);
        if (location == null)
        {
            return NotFound();
        }

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    private bool LocationExists(int id)
    {
        return _context.Locations.Any(e => e.Id == id);
    }
    
}