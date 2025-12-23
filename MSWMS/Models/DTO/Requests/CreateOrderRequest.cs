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

    public async Task<Order> ToEntity(AppDbContext context, DCXWMSContext dcxContext)
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

        // Find items without ItemInfo
        var itemsWithoutInfo = items
            .Select((item, index) => new { item, index })
            .Where(x => x.item.ItemInfo == null || x.item.ItemInfo.Count == 0)
            .ToList();

        if (itemsWithoutInfo.Any())
        {
            var missingPairs = itemsWithoutInfo
                .Select(x => new { x.item, Request = Items.ElementAt(x.index) })
                .Select(x => new { x.Request.ItemNumber, x.Request.Variant })
                .ToList();

            var missingItemNumbers = missingPairs.Select(p => p.ItemNumber).Distinct().ToList();
            var missingVariants = missingPairs.Select(p => p.Variant).Distinct().ToList();

            var crossReferences = await dcxContext.DcxMsItemCrossReference
                .Where(cr => missingItemNumbers.Contains(cr.ItemNo) 
                             && (cr.VariantCode == "" || missingVariants.Contains(cr.VariantCode)))
                .ToListAsync();

            foreach (var entry in itemsWithoutInfo)
            {
                var itemWithoutInfo = entry.item;
                var request = Items.ElementAt(entry.index);


                var crossRef = crossReferences
                    .FirstOrDefault(cr => cr.ItemNo == request.ItemNumber 
                                         && cr.VariantCode == request.Variant);

                if (crossRef != null)
                {
                    var newItemInfo = new ItemInfo
                    {
                        Barcode = crossRef.CrossReferenceNo,
                        ItemNumber = crossRef.ItemNo,
                        Variant = string.IsNullOrEmpty(crossRef.VariantCode) ? null : crossRef.VariantCode,
                        Description = crossRef.Description,
                        Price = 0
                    };

                    context.ItemInfos.Add(newItemInfo);
                    await context.SaveChangesAsync();

                    itemWithoutInfo.ItemInfo = new List<ItemInfo> { newItemInfo };
                }
            }
        }
        
        

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