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
    public int TotalQuantity { get; set; }
    public int TotalScanned { get; set; }
    public int TotalRemaining { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public string? Remark { get; set; }
    public string? Type { get; set; }
}