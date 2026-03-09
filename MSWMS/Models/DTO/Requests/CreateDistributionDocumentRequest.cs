namespace MSWMS.Models.DTO.Requests;

public class CreateDistributionDocumentRequest
{
    public required int DistributionId { get; set; }
    public required string DocumentNumber { get; set; }
    public required string OrderNumber { get; set; }
    public int OriginId { get; set; }
    public int DestinationId { get; set; }
}