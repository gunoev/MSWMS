namespace MSWMS.Models.DTO.Requests;

public class UpdateDistributionDocumentRequest
{
    public string? DocumentNumber { get; set; }
    public string? OrderNumber { get; set; }
    public int? OriginId { get; set; }
    public int? DestinationId { get; set; }
}
