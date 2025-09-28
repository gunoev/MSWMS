using MSWMS.Entities;
using MSWMS.Models.Auth;

namespace MSWMS.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResult> Login(LoginModel model);
    Task<AuthResult> Register(RegisterModel model);
}

public class AuthResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
    public User? User { get; set; }
}
