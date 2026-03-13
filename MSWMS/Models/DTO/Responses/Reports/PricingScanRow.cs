namespace MSWMS.Models.DTO.Responses.Reports;

public class PricingScanRow
{
    public DateTime TimeStamp { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string ItemNumber { get; set; } = string.Empty;
    public string Variant { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string ShipmentNumber { get; set; } = string.Empty;
    public string OrderTotalQuantity { get; set; } = string.Empty;
    public string OrderTotalScanned { get; set; } = string.Empty;
    public string OrderTotalBoxes { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string OriginCode { get; set; } = string.Empty;
    public string DestinationCode { get; set; } = string.Empty;
    public string BoxId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int BoxNumber { get; set; }
}