using System.ComponentModel.DataAnnotations;

namespace MSWMS.Entities;

public class Shipment
{
    public int Id { get; set; }
    public required User CreatedBy { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public required Location Origin { get; set; }
    public required Location Destination { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Scheduled { get; set; }
    public bool IsCompleted { get; set; }
    [MaxLength(256)]
    public string TransportPlateNumber { get; set; } = string.Empty;
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Scheduled;
    public ICollection<ShipmentEvent>? Events { get; set; }
}

public enum ShipmentStatus
{
    Scheduled,
    Shipped,
    Delivered,
    Cancelled,
}