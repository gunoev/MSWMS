namespace MSWMS.Models.Responses;

public class ShipmentDto
{
    public int Id { get; set; }
    public required string Origin { get; set; }
    public required string Destination { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime Scheduled { get; set; }
    public int TotalBoxes { get; set; }
    public bool IsCompleted { get; set; }
    public ICollection<ShipmentOrderDto>? Orders { get; set; }
}