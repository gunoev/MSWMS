namespace MSWMS.Entities;

public class ItemInfo
{
    public int Id { get; set; }
    public required string Barcode { get; set; }
    public required string ItemNumber { get; set; }
    public string? Variant { get; set; }
    public string? Description { get; set; }
    public string? UnitOfMeasure { get; set; }
    public string? Na { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public string? Currency { get; set; }
}