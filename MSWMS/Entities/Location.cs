namespace MSWMS.Entities;

public class Location
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? ShortName { get; set; }
    public string? Code { get; set; }
    public ICollection<Order>? OriginOrders { get; set; }
    public ICollection<Order>? DestinationOrders { get; set; }
}