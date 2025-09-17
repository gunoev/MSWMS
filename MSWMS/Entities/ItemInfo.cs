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
    public double Price { get; set; }
    public double DiscountPrice { get; set; }
    public double DiscountPercentage { get; set; }
    public string? Currency { get; set; }
}