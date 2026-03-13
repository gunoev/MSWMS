namespace MSWMS.Models.DTO.Responses.Distributions;

public class DistributionItemDto
{
    public int Id { get; set; }
    public string ItemNumber { get; set; } = string.Empty;
    public string? Variant { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public string BinCode { get; set; } = string.Empty;
    public string LotNumber { get; set; } = string.Empty;
    public int DocumentId { get; set; }
    public string DestinationCode { get; set; } = string.Empty;
    public string DestinationName { get; set; } = string.Empty;
    public int DestinationId { get; set; }
}
