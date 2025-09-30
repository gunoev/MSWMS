using MSWMS.Entities;

namespace MSWMS.Models.Responses;

public class OrderDto
{
    public int Id { get; set; }
    public required string ShipmentId { get; set; }
    public required string TransferOrderNumber { get; set; }
    public required string TransferShipmentNumber { get; set; }
    public required string Origin { get; set; }
    public string? Destination { get; set; }
    public string? OriginCode { get; set; }
    public required string DestinationCode { get; set; }
    public required Order.OrderStatus Status { get; set; }
    public required Order.OrderType Type { get; set; }
    public required Order.OrderPriority Priority { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public required int TotalQuantity { get; set; }
    public required int TotalScanned { get; set; }
    public required int TotalBoxes { get; set; }
    
    public OrderDto FromEntity(Order order)
    {
        Id = order.Id;
        ShipmentId = order.ShipmentId;
        TransferOrderNumber = order.TransferOrderNumber;
        TransferShipmentNumber = order.TransferShipmentNumber;
        Origin = order.Origin.Name;
        Destination = order.Destination.Name;
        Status = order.Status;
        OriginCode = order.Origin.Code;
        DestinationCode = order.Destination.Code;
        Type = order.Type;
        Priority = order.Priority;
        CreatedAt = order.CreatedDateTime;
        UpdatedAt = order.LastChangeDateTime;
        CreatedBy = order.CreatedBy?.Name ?? "Unknown";
        TotalBoxes = order.Boxes?.Count ?? 0;
        TotalQuantity = order.Items.Sum(i => i.NeededQuantity);
        TotalScanned = order.Scans?.Count ?? 0;

        return this;
    }
}