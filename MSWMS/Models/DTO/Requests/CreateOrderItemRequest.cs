using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;

namespace MSWMS.Models.Requests;

public class CreateOrderItemRequest
{
    public required string Barcode { get; set; }
    public required string ItemNumber { get; set; }
    public string? Variant { get; set; }
    public string? Description { get; set; }
    public required int NeededQuantity { get; set; }

    public async Task<Item> ToEntity(AppDbContext context)
    {
        var itemInfo = await context
            .ItemInfos
            .Where(inf => inf.Variant == Variant && inf.ItemNumber == ItemNumber)
            .ToListAsync();
    
        var item = new Item
        {
            NeededQuantity = NeededQuantity,
            ItemInfo = itemInfo,
        };

        return item;
    }
}