using System.ComponentModel.DataAnnotations;

namespace MSWMS.Entities;

public class Role
{
    public int Id { get; set; }
    [MaxLength(128)]
    public string? Name { get; set; }
    public required RoleType Type { get; set; }
    public ICollection<User>? Users { get; set; }
    
    public enum RoleType
    {
        Admin,
        Picker,
        Manager,
        Observer,
        LoadingOperator,
        Dispatcher,
        Unknown,
    }
}