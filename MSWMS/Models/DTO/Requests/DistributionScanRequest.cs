using System.ComponentModel.DataAnnotations;
using MSWMS.Models.DTO.Responses.Distributions;

namespace MSWMS.Models.DTO.Requests;

public class DistributionScanRequest
{
    [MaxLength(100)]
    public required string Barcode { get; set; }
    public required DistributionItemDto Item { get; set; }
    public int DocumentId { get; set; }
    public int DistributionId { get; set; }
    public int UserId { get; set; }
}
