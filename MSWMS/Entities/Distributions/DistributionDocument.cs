using System.ComponentModel.DataAnnotations;

namespace MSWMS.Entities.Distributions;

public class DistributionDocument : BaseEntity
{
    [MaxLength(100)]
    public required string DocumentNumber { get; set; }
    
    [MaxLength(50)]
    public required string OrderNumber { get; set; }
    
    public ICollection<DistributionItem> Items { get; set; } = new List<DistributionItem>();
    
    public int OriginId { get; set; }
    
    public int DistributionId { get; set; }
    
    public Location? Origin { get; set; }

    public Distribution? Distribution { get; set; }
}