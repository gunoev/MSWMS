namespace MSWMS.Infrastructure.Helpers;

public class ExcelParsedItem
{
    public required string Barcode { get; set; }
    public required string ItemNumber { get; set; }
    public string? Variant { get; set; }
    public string? Description { get; set; }
    public required int NeededQuantity { get; set; }
}