using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Infrastructure.Authorization;

namespace MSWMS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = Policies.RequireAdmin)]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    
    public AdminController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet("test")]
    [AllowAnonymous]
    public IActionResult Test()
    {
        return Ok("Test endpoint works!");
    }
    
    [HttpGet("users")]
    [Authorize(Policy = Policies.RequireAdmin)]
    public IActionResult GetAllUsers()
    {
        var users = _dbContext.Users
            .Include(u => u.Roles)
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Name,
                u.Email,
                u.Status,
                Roles = u.Roles.Select(r => r.Type.ToString())
            })
            .ToList();
        
        return Ok(users);
    }
}