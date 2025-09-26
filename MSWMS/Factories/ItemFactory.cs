using MSWMS.Entities;

namespace MSWMS.Factories;

public static class ItemFactory
{
    public static Item Create(int neededQuantity, ICollection<ItemInfo> itemInfo)
    {
        return new Item
        {
            NeededQuantity = neededQuantity,
            ItemInfo = itemInfo
        };
    }
}