namespace MSWMS.Entities;

public class Shipment
{
    public int Id { get; set; }
    public required User CreatedBy { get; set; }
    public required ICollection<Order> Orders { get; set; }
    public required Location Origin { get; set; }
    public required Location Destination { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Scheduled { get; set; }
    public bool IsCompleted { get; set; }
    public ICollection<ShipmentEvent>? Events { get; set; }
}