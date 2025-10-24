namespace MSWMS.Models.Requests;

public class CreateShipmentRequest
{
    public required int LocationId { get; set; }
    public required ICollection<int> OrderIds { get; set; }
    public required DateTime Scheduled { get; set; }
}