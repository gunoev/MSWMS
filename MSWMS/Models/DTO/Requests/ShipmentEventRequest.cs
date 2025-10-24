using System.ComponentModel.DataAnnotations;
using MSWMS.Entities;

namespace MSWMS.Models.Requests;

public class ShipmentEventRequest
{
    public required int ShipmentId { get; set; }
    [MaxLength(256)]
    public required string Code { get; set; }
    public required ShipmentEvent.ShipmentAction Action { get; set; }
}