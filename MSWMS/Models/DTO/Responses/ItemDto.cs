namespace MSWMS.Models.Responses;

public class ItemDto
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public string ItemNumber { get; set; }
    public string Variant { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public uint Quantity { get; set; }
    public int Remaining { get; set; }
    public uint Scanned { get; set; }
}