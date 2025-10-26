using System.ComponentModel.DataAnnotations;

namespace MSWMS.Entities;

public class ShipmentEvent
{
    public int Id { get; set; }
    [MaxLength(256)] public required string Code { get; set; }
    public DateTime Timestamp { get; set; }
    public required Location Location { get; set; }
    public Box? Box { get; set; }
    public required User User { get; set; }
    public required ShipmentAction Action { get; set; }
    public required EventStatus Status { get; set; }

    public enum EventStatus
    {
        Ok,
        NotFound,
        Duplicate,
        Error,
    }

    public enum ShipmentAction
    {
        Register,
        Load,
        Unload,
        Check
    }
}