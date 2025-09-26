namespace MSWMS.Infrastructure.Helpers;

public class ExcelParsedOrder
{
    public string? ShipmentId { get; set; }
    public string? TransferOrderNumber { get; set; }
    public string? TransferShipmentNumber { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public string? OriginCode { get; set; }
    public string? DestinationCode { get; set; }
    public ICollection<ExcelParsedItem>? Items { get; set; }
}