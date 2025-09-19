namespace MSWMS.Entities;

public class Box
{
    public int Id { get; set; }
    public int BoxNumber { get; set; }
    public required string GlobalUid { get; init; } = Guid.NewGuid().ToString();
    public string? UniqueId { get; set; }
    public required Order Order { get; set; }
    public required User User { get; set; }
    
}