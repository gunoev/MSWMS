using System.ComponentModel.DataAnnotations;

namespace MSWMS.Models.Auth;

public class LoginModel
{
    [Required(ErrorMessage = "Login required")]
    public required string Username { get; set; }
    
    [Required(ErrorMessage = "Password required")]
    public required string Password { get; set; }
}
