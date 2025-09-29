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

    public Order ToEntity(CreateOrderRequest dto)
    {
        using var context = new AppDbContext();
        
        var origin = context.Locations.Find(dto.OriginId);
        var destination = context.Locations.Find(dto.DestinationId);
        var user = context.Users.Find(dto.UserId);
        
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
        
        foreach (var item in dto.Items)
        {
            items.Add(item.ToEntity(item));
        }
        var order = new Order
        {
            ShipmentId = dto.ShipmentId,
            TransferOrderNumber = dto.TransferOrderNumber,
            TransferShipmentNumber = dto.TransferShipmentNumber,
            Status = Order.OrderStatus.New,
            CreatedDateTime = DateTime.Now,
            Priority = dto.Priority,
            Remark = dto.Remark,
            Items = items,
            Origin = origin,
            Destination = destination,
            CreatedBy = user,
            Type = dto.Type,
        };

        return order;
    }
    
}