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
using MSWMS.Models.Auth;
using MSWMS.Services;
using MSWMS.Services.Interfaces;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        
        public UserController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Policy = Policies.RequireManager)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [Authorize(Policy = Policies.RequireManager)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = Policies.RequireManager)]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = Policies.RequireManager)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
        
        [HttpPost("add-user")] // NOT READY
        [Authorize(Policy = Policies.RequireManager)]
        public async Task<IActionResult> AddNewUser([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            var result = await _authService.Register(model);
        
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }
        
            return Ok(new 
            {
                Token = result.Token,
                User = new 
                {
                    result.User.Id,
                    result.User.Name,
                    result.User.Username,
                    result.User.Email,
                    Roles = result.User.Roles.Select(r => r.Type.ToString())
                }
            });
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.RequireAdmin)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
