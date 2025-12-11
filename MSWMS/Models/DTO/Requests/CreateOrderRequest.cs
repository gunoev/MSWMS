using Microsoft.EntityFrameworkCore;
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
        var origin = await context.Locations.FindAsync(OriginId);
        var destination = await context.Locations.FindAsync(DestinationId);
        var user = await context.Users.FindAsync(UserId);

        if (origin is null) throw new Exception("Origin location not found");
        if (destination is null) throw new Exception("Destination location not found");
        if (user is null) throw new Exception("User not found");

        var neededPairs = Items
            .Select(i => new { i.ItemNumber, i.Variant })
            .ToList();

        var itemNumbers = neededPairs.Select(p => p.ItemNumber).Distinct().ToList();
        var variants = neededPairs.Select(p => p.Variant).Distinct().ToList();

        var allInfos = await context.ItemInfos
            .Where(inf => itemNumbers.Contains(inf.ItemNumber)
                          && (inf.Variant == null || variants.Contains(inf.Variant))).ToListAsync();
        
        
        var items = Items.Select(req =>
        {
            var info = allInfos
                .Where(inf => inf.ItemNumber == req.ItemNumber && inf.Variant == req.Variant)
                .ToList();

            return new Item
            {
                NeededQuantity = req.NeededQuantity,
                ItemInfo = info
            };
        }).ToList();

        return new Order
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
    }
}