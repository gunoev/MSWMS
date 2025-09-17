namespace MSWMS.Entities;

public class Role
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public required RoleType Type { get; set; }
    public required ICollection<User> Users { get; set; }
    
    public enum RoleType
    {
        Admin,
        Picker,
        Manager,
        Observer,
        Dispatcher,
        Unknown,
    }
}