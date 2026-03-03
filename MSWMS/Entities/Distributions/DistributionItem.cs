using System.ComponentModel.DataAnnotations;

namespace MSWMS.Entities.Distributions;

public class DistributionItem : BaseEntity
{
    [MaxLength(50)]
    public required string ItemNumber { get; set; }
    
    [MaxLength(50)]
    public string? Variant { get; set; }
    
    [MaxLength(256)]
    public string? Description { get; set; }
    
    public int Quantity { get; set; }
    
    [MaxLength(50)]
    public required string BinCode { get; set; }
    
    [MaxLength(50)]
    public required string LotNumber { get; set; }
    
    public int DocumentId { get; set; }
    
    public required Location Destination { get; set; }
    
    public required DistributionDocument Document { get; set; }
}