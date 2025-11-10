using MSWMS.Entities;

namespace MSWMS.Models.Responses;

public class ShipmentOrderDto
{
    public int Id { get; set; }
    public required int ShipmentId { get; set; }
    public string? DbCode { get; set; }
    public required string TransferOrderNumber { get; set; }
    public required string TransferShipmentNumber { get; set; }
    public required string Origin { get; set; }
    public required string Destination { get; set; }
    public int Boxes { get; set; }
    public int LoadedBoxes { get; set; }
    public int TotalItems { get; set; }
    public int TotalCollectedItems { get; set; }
    public int TotalLoadedItems { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public string? Remark { get; set; }
    public string? Type { get; set; }
    
    public ShipmentOrderDto FromEntity(Order order)
    {
        Id = order.Id;
        DbCode = order.ShipmentId;
        TransferOrderNumber = order.TransferOrderNumber;
        TransferShipmentNumber = order.TransferShipmentNumber;
        Origin = order.Origin.Name;
        Destination = order.Destination.Name;
        Status = order.Status.ToString();
        Type = order.Type.ToString();
        Priority = order.Priority.ToString();
        Boxes = order.Boxes?.Count ?? 0;
        LoadedBoxes = order.Shipments.SelectMany(s => s.Events)
            .Count(e => e.Order.Id == order.Id && e.Action == ShipmentEvent.ShipmentAction.Load);
        TotalItems = order.Items.Sum(i => i.NeededQuantity);
        TotalCollectedItems = order.Scans?.Count ?? 0;

        return this;
    }
}