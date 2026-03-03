using System.ComponentModel.DataAnnotations;

namespace MSWMS.Entities.Distributions;

public class Distribution : BaseEntity
{
    public DateOnly Date { get; set; }
    [MaxLength(255)]
    public string Note { get; set; } = string.Empty;
    public ICollection<DistributionDocument> Documents { get; set; } = new List<DistributionDocument>();
    public ICollection<DistributionScan> Scans { get; set; } = new List<DistributionScan>();
}