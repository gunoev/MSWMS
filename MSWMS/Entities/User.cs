namespace MSWMS.Entities;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public required string Username { get; set; }
    public string? Email { get; set; }
    public required string PasswordHash { get; set; }
    public required UserStatus Status { get; set; } = UserStatus.Unknown;
    public required Location Location { get; set; }
    public required ICollection<Role> Roles { get; set; }
    
    public enum UserStatus
    {
        Active,
        Inactive,
        Deleted,
        Unknown,
    }
}