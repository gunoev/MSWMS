using MSWMS.Entities;

namespace MSWMS.Models.Responses;

public class ShipmentEventDto
{
    public int Id { get; set; }
    public required int ShipmentId { get; set; }
    public DateTime Timestamp { get; set; }
    public required string Code { get; set; }
    public required string Location { get; set; }
    public BoxDto? Box { get; set; }
    public ShipmentOrderDto? Order { get; set; }
    public required string User { get; set; }
    public required ShipmentEvent.ShipmentAction Action { get; set; }
    public required ShipmentEvent.EventStatus Status { get; set; }
}