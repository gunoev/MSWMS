using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Models.Auth;
using MSWMS.Services;
using MSWMS.Services.Interfaces;

namespace MSWMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly AppDbContext _context;
    
    public AuthController(IAuthService authService, AppDbContext context)
    {
        _authService = authService;
        _context = context;
    }
    
    // В методе Login в AuthController
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        var result = await _authService.Login(model);
    
        if (!result.Success)
        {
            return Unauthorized(new { Message = result.Message });
        }
    
        // Явно вызываем создание куки с правильными настройками
        await SignInWithCookies(result.User);
    
        // Добавим заголовок для отладки
        Response.Headers.Add("X-Auth-Debug", "Cookie-Set-Attempted");
        
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
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
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
        
        // Создаем куки и JWT токен
        await SignInWithCookies(result.User);
        
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
    
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
        return Ok(new 
        {
            Username = User.Identity?.Name,
            Email = user?.Email,
            Name = user?.Name,
            Roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
        });
    }
    
    [HttpPost("logout")]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { Message = "Successfully logged out" });
    }
    
    private async Task SignInWithCookies(Entities.User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };
        
        // Добавляем роли пользователя
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Type.ToString()));
        }
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
        };
        
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
}