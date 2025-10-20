namespace MSWMS.Entities;

public class Shipment
{
    public int Id { get; set; }
    public required User CreatedBy { get; set; }
    public required Order Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Scheduled { get; set; }
    public ICollection<ShipmentEvent>? Events { get; set; }
}