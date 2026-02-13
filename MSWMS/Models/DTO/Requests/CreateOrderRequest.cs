using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Entities.External;
using System.Globalization;
using System.Text;

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

    public async Task<Order> ToEntity(AppDbContext context, DCXWMSContext dcxContext, ExternalReadOnlyContext externalContext)
    {
        var origin = await context.Locations.FindAsync(OriginId);
        var destination = await context.Locations.FindAsync(DestinationId);
        var user = await context.Users.FindAsync(UserId);

        if (origin is null) throw new Exception("Order not created. Origin location not found.");
        if (destination is null) throw new Exception("Order not created. Destination location not found.");
        if (user is null) throw new Exception("Order not created. User not found");

        static string? NormVariant(string? v)
        {
            if (string.IsNullOrWhiteSpace(v)) return null;

            // Убираем невидимые Unicode Format chars (U+202D/U+202C)
            var cleaned = string.Concat(
                v.EnumerateRunes()
                 .Where(r => Rune.GetUnicodeCategory(r) != UnicodeCategory.Format)
                 .Select(r => r.ToString())
            );

            cleaned = cleaned.Trim();
            return cleaned.Length == 0 ? null : cleaned.ToUpperInvariant();
        }

        var itemsList = Items.ToList();

        var neededPairs = itemsList
            .Select(i => new { i.ItemNumber, Variant = NormVariant(i.Variant) })
            .ToList();

        var itemNumbers = neededPairs.Select(p => p.ItemNumber).Distinct().ToList();

        var allInfos = await context.ItemInfos
            .Where(inf => itemNumbers.Contains(inf.ItemNumber))
            .ToListAsync();

        var items = itemsList.Select(req =>
        {
            var reqVariant = NormVariant(req.Variant);

            var info = allInfos
                .Where(inf =>
                    inf.ItemNumber == req.ItemNumber &&
                    NormVariant(inf.Variant) == reqVariant)
                .ToList();

            return new Item
            {
                NeededQuantity = req.NeededQuantity,
                ItemInfo = info
            };
        }).ToList();

        var itemsWithoutInfo = items
            .Select((item, index) => new { item, index })
            .Where(x => x.item.ItemInfo == null || x.item.ItemInfo.Count == 0)
            .ToList();

        if (itemsWithoutInfo.Any())
        {
            var missingPairs = itemsWithoutInfo
                .Select(x => new { x.item, Request = itemsList[x.index] })
                .Select(x => new { x.Request.ItemNumber, Variant = NormVariant(x.Request.Variant) })
                .ToList();

            var missingItemNumbers = missingPairs.Select(p => p.ItemNumber).Distinct().ToList();
            
            var crossReferences = await dcxContext.DcxMsItemCrossReference
                .Where(cr => missingItemNumbers.Contains(cr.ItemNo))
                .ToListAsync();

            var crossRefItemNos = crossReferences
                .Select(cr => cr.ItemNo)
                .Distinct()
                .ToList();

            var salesPrices = await externalContext.MikesportCoSALDefaultSalesPrices
                .AsNoTracking()
                .Where(sp => crossRefItemNos.Contains(sp.ItemNo))
                .ToListAsync();

            var salesPriceByItemNo = salesPrices
                .GroupBy(sp => sp.ItemNo)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());

            foreach (var entry in itemsWithoutInfo)
            {
                var itemWithoutInfo = entry.item;
                var request = itemsList[entry.index];

                var reqVariant = NormVariant(request.Variant);

                // ВАЖНО: берём ВСЕ совпадения, а не FirstOrDefault
                var crossRefs = crossReferences
                    .Where(cr =>
                        cr.ItemNo == request.ItemNumber &&
                        NormVariant(cr.VariantCode) == reqVariant)
                    .ToList();

                if (crossRefs.Count == 0)
                {
                    throw new Exception(
                        $"Order not created. No item info found for itemNumber='{request.ItemNumber}', variant='{request.Variant}'.");
                }

                var newInfos = new List<ItemInfo>(capacity: crossRefs.Count);

                foreach (var crossRef in crossRefs)
                {
                    salesPriceByItemNo.TryGetValue(crossRef.ItemNo, out var salesPrice);

                    var newItemInfo = new ItemInfo
                    {
                        Barcode = crossRef.CrossReferenceNo,
                        ItemNumber = crossRef.ItemNo,
                        Variant = NormVariant(crossRef.VariantCode),
                        Description = crossRef.Description,
                        Price = salesPrice?.UnitPriceIncludingVatCurre ?? 0,
                        DiscountPrice = salesPrice?.DiscountedPriceCurrency ?? 0,
                        Currency = salesPrice?.Currency ?? "",
                        DiscountPercentage = salesPrice?.DiscountPercentage ?? 0
                    };

                    context.ItemInfos.Add(newItemInfo);
                    newInfos.Add(newItemInfo);
                }

                itemWithoutInfo.ItemInfo = newInfos;
            }

            await context.SaveChangesAsync();
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