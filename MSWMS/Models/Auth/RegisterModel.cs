using System.ComponentModel.DataAnnotations;
using MSWMS.Entities;


namespace MSWMS.Models.Auth;

public class RegisterModel
{
    [Required(ErrorMessage = "Имя пользователя обязательно")] // убрать required
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Login required")]
    public required string Username { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public required string Password { get; set; }
    
    [Required(ErrorMessage = "Password confirmation required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public required string ConfirmPassword { get; set; }
    
    public ICollection<Role>? Roles { get; set; }
}
