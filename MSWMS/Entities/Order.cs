namespace MSWMS.Entities;

public class Order
{
    public int Id { get; set; }
    public required string TransferOrderNumber { get; set; }
    public required string TransferShipmentNumber { get; set; }
    public required Location Origin { get; set; }
    public required Location Destination { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.New;
    public DateTime? CollectedDateTime { get; set; }
    public DateTime? LastChangeDateTime { get; set; }
    public DateTime CreatedDateTime { get; init; } = DateTime.Now;
    public OrderPriority Priority { get; set; } = OrderPriority.Medium;
    public required string ShipmentId { get; set; }
    public string? Remark { get; set; }
    public required OrderType Type { get; set; }
    public required User CreatedBy { get; set; }
    
    public ICollection<User>? CollectedBy { get; set; }
    public required ICollection<Item> Items { get; set; }
    public ICollection<Box>? Boxes { get; set; }
    public ICollection<Scan>? Scans { get; set; }
    public ICollection<User>? Users { get; set; }
    public ICollection<Shipment>? Shipments { get; set; }
    
    
    public enum OrderStatus
    {
        New,
        InProgress,
        Collected,
    }

    public enum OrderType
    {
        Refill,
        Distribution,
        Other,
        SalesOrder,
        TransferOrder
    }

    public enum OrderPriority
    {
        Low,
        Medium,
        High,
    }
}