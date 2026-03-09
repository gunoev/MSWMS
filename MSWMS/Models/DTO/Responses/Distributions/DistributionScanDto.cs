using MSWMS.Entities.Distributions;

namespace MSWMS.Models.DTO.Responses.Distributions;

public class DistributionScanDto
{
    public int Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string BinCode { get; set; } = string.Empty;
    public string LotNumber { get; set; } = string.Empty;
    public ScanType ScanType { get; set; }
    public DateTime TimeStamp { get; set; }
    public DistributionScanStatus Status { get; set; }
    public int UserId { get; set; }
    public int OriginId { get; set; }
    public int DistributionId { get; set; }
    public int? DocumentId { get; set; }
    public int? ItemId { get; set; }
}
