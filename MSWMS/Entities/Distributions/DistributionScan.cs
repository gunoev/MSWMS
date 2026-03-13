using System.ComponentModel.DataAnnotations;

namespace MSWMS.Entities.Distributions;

public class DistributionScan : BaseEntity
{
    [MaxLength(100)]
    public required string Barcode { get; set; }
    
    public DistributionItem? Item { get; set; }
    
    [MaxLength(50)]
    public required string BinCode { get; set; }
    
    [MaxLength(50)]
    public required string LotNumber { get; set; }
    
    public ScanType ScanType { get; set; } = ScanType.Take;
    
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    
    public DistributionScanStatus Status { get; set; } = DistributionScanStatus.Ok;
    
    public int UserId { get; set; }
    
    public int OriginId { get; set; }
    
    public int? DocumentId { get; set; }
    
    public int DistributionId { get; set; }
    
    public int? ItemId { get; set; }
    
    public User? User { get; set; }
    
    public Location? Origin { get; set; }
    
    public DistributionDocument? Document { get; set; }
    
    public Distribution? Distribution { get; set; }
}