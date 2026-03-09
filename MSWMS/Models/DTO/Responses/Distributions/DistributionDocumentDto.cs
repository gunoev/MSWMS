namespace MSWMS.Models.DTO.Responses.Distributions;

public class DistributionDocumentDto
{
    public int Id { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public int OriginId { get; set; }
    public int DestinationId { get; set; }
    public int DistributionId { get; set; }
    public int ItemsCount { get; set; }
}
