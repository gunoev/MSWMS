namespace MSWMS.Entities;

public class Box
{
    public int Id { get; set; }
    public int BoxNumber { get; set; }
    public string? UniqueId { get; set; }
    public required Order Order { get; set; }
    
}