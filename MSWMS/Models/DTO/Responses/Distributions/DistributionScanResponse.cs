namespace MSWMS.Models.DTO.Responses.Distributions;

public class DistributionScanResponse
{
    public DistributionScanDto Scan { get; set; }
    public DistributionItemDto? Item { get; set; }
    
}