using MSWMS.Entities;

namespace MSWMS.Models.Requests;

public class CreateOrderRequest
{
    public required string ShipmentId { get; set; }
    public required string TransferOrderNumber { get; set; }
    public required string TransferShipmentNumber { get; set; }
    public required int OriginId { get; set; }
    public required int DestinationId { get; set; }
    public required int UserId { get; set; }
    public required Order.OrderType Type { get; set; }
    public required Order.OrderPriority Priority { get; set; }
    public string? Remark { get; set; }
    public required ICollection<CreateOrderItemRequest> Items { get; set; }

    public async Task<Order> ToEntity(AppDbContext context)
    {
        var origin = context.Locations.Find(OriginId);
        var destination = context.Locations.Find(DestinationId);
        var user = context.Users.Find(UserId);
        
        if (origin is null)
        {
            throw new Exception("Origin location not found");
        }
        if (destination is null)
        {
            throw new Exception("Destination location not found");
        }

        if (user is null)
        {
            throw new Exception("User not found");
        }
        
        var items = new List<Item>();
        
        foreach (var item in Items)
        {
            items.Add(await item.ToEntity(context));
        }
        var order = new Order
        {
            ShipmentId = ShipmentId,
            TransferOrderNumber = TransferOrderNumber,
            TransferShipmentNumber = TransferShipmentNumber,
            Status = Order.OrderStatus.New,
            CreatedDateTime = DateTime.Now,
            Priority = Priority,
            Remark = Remark,
            Items = items,
            Origin = origin,
            Destination = destination,
            CreatedBy = user,
            Type = Type,
        };

        return order;
    }
    
}