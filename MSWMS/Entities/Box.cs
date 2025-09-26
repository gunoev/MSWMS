namespace MSWMS.Entities;

public class Box
{
    public int Id { get; set; }
    public int BoxNumber { get; set; }
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string? UniqueId { get; set; }
    public required Order Order { get; set; }
    public required User User { get; set; }
    
}