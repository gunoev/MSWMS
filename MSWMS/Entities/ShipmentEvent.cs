namespace MSWMS.Entities;

public class ShipmentEvent
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public DateTime Timestamp { get; set; }
    public required Location Location { get; set; }
    public Box? Box { get; set; }
    public required User User { get; set; }
    public required ShipmentAction Action { get; set; }
}

public enum ShipmentAction
{
    Register,
    Load,
    Unload,
    Check
}