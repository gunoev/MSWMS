using System.ComponentModel.DataAnnotations;

namespace MSWMS.Entities.Distributions;

public class DistributionScan
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
    
    public ScanStatus Status { get; set; } = ScanStatus.Ok;
    
    public int UserId { get; set; }
    
    public int OriginId { get; set; }
    
    public int? DocumentId { get; set; }
    
    public int DistributionId { get; set; }
    
    public int? ItemId { get; set; }
    
    public required User User { get; set; }
    
    public required Location Origin { get; set; }
    
    public DistributionDocument? Document { get; set; }
    
    public required Distribution Distribution { get; set; }
}