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

    public Item ToEntity()
    {
        using var context = new AppDbContext();
        var itemInfo = context.ItemInfos.Where(inf => inf.Variant == Variant && inf.ItemNumber == ItemNumber).ToListAsync().Result;
        
        var item = new Item
        {
            NeededQuantity = NeededQuantity,
            ItemInfo = itemInfo,
        };

        return item;
    }
}