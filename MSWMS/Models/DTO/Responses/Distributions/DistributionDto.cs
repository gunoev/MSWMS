namespace MSWMS.Models.DTO.Responses.Distributions;

public class DistributionDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Note { get; set; } = string.Empty;
    public int DocumentsCount { get; set; }
    public int ScansCount { get; set; }
}
