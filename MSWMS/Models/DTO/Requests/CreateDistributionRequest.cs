using System.ComponentModel.DataAnnotations;

namespace MSWMS.Models.DTO.Requests;

public class CreateDistributionRequest
{
    public required DateOnly Date { get; set; }
    [MaxLength(255)]
    public string Note { get; set; } = string.Empty;
}