using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSWMS.Entities;
using MSWMS.Models.Auth;
using MSWMS.Services.Interfaces;

namespace MSWMS.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    
    public AuthService(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }
    
    [AllowAnonymous]
    public async Task<AuthResult> Login(LoginModel model)
    {
        var user = await _dbContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Username == model.Username);
        
        if (user == null)
        {
            return new AuthResult
            {
                Success = false,
                Message = "User not found"
            };
        }
        
        // Проверка пароля (хеша)
        if (!VerifyPasswordHash(model.Password, user.PasswordHash))
        {
            return new AuthResult
            {
                Success = false,
                Message = "Wrong password"
            };
        }
        
        // Проверка статуса пользователя
        if (user.Status != User.UserStatus.Active)
        {
            return new AuthResult
            {
                Success = false,
                Message = "Inactive account"
            };
        }
        
        var token = GenerateJwtToken(user);
        
        return new AuthResult
        {
            Success = true,
            Token = token,
            User = user
        };
    }
    
    public async Task<AuthResult> Register(RegisterModel model)
    {
        // Проверка существования пользователя
        if (await _dbContext.Users.AnyAsync(u => u.Username == model.Username))
        {
            return new AuthResult
            {
                Success = false,
                Message = "User with this username already exists"
            };
        }
        
        if (!string.IsNullOrEmpty(model.Email) && 
            await _dbContext.Users.AnyAsync(u => u.Email == model.Email))
        {
            return new AuthResult
            {
                Success = false,
                Message = "User with this email already exists"
            };
        }
        
        Location? defaultLocation;

        if (model.LocationId == 0)
        {
            defaultLocation = await _dbContext.Locations.FirstOrDefaultAsync() 
                                  ?? throw new InvalidOperationException("Default location not found.");   
        }
        else
        {
            defaultLocation = await _dbContext.Locations.FindAsync(model.LocationId)
                              ?? throw new InvalidOperationException("Location not found.");
        }
        
        var user = new User
        {
            Username = model.Username,
            PasswordHash = HashPassword(model.Password),
            Email = model.Email,
            Name = model.Name,
            Status = User.UserStatus.Active,
            Location = defaultLocation,
            Roles = new List<Role>()
        };

        if (model.Roles != null && model.Roles.Any())
        {
            var roles = await _dbContext.Roles
                .Where(r => model.Roles.Contains(r.Type))
                .ToListAsync();
                
            foreach(var role in roles)
            {
                user.Roles.Add(role);
            }
        }
        else
        {
            var defaultRole = await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Type == Role.RoleType.Observer);
        
            if (defaultRole != null)
            {
                user.Roles.Add(defaultRole);
            }
        }
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var token = GenerateJwtToken(user);
        
        return new AuthResult
        {
            Success = true,
            Token = token,
            User = user
        };
    }
    
    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        // Добавление ролей в claims
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Type.ToString()));
        }
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key no set up")
        ));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private string HashPassword(string password)
    {
        // В реальном приложении используйте более надежный алгоритм хеширования паролей, 
        // например, BCrypt или PBKDF2
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    private bool VerifyPasswordHash(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}