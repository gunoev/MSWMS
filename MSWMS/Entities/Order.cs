namespace MSWMS.Entities;

public class Order
{
    public int Id { get; set; }
    public required string TransferOrderNumber { get; set; }
    public required string TransferShipmentNumber { get; set; }
    public required string TransferTo { get; set; }
    public required string TransferFrom { get; set; }
    public required OrderStatus Status { get; set; } = OrderStatus.New;
    public DateTime? CollectedDateTime { get; set; }
    public DateTime? LastChangeDateTime { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public int Priority { get; set; }
    public required string ShipmentId { get; set; }
    public string? Remark { get; set; }
    public required OrderType Type { get; set; }
    
    public ICollection<User>? CompletedBy { get; set; }
    public required ICollection<Item> Items { get; set; }
    public required ICollection<Box> Boxes { get; set; }
    public required ICollection<Scan> Scans { get; set; }
    public required ICollection<User> Users { get; set; }
    
    
    public enum OrderStatus
    {
        New,
        InProgress,
        Completed,
    }

    public enum OrderType
    {
        Refill,
        Distribution,
        Other,
    }
}