namespace MSWMS.Entities;

public class Item
{
    public int Id { get; set; }
    public int NeededQuantity { get; set; }
    public required ICollection<ItemInfo> ItemInfo { get; set; }
}