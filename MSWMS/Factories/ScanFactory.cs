using MSWMS.Entities;

namespace MSWMS.Factories;

public static class ScanFactory
{
    private static Scan Create(string barcode, Item item, Box box, Order order, User user, Scan.ScanStatus status)
    {
        return new Scan
        {
            Barcode = barcode,
            TimeStamp = DateTime.Now,
            Status = status,
            Item = item,
            Box = box,
            Order = order,
            User = user
        };
    }
    
    public static Scan CreateOk(string barcode, Item item, Box box, Order order, User user)
    {
        return Create(barcode, item, box, order, user, Scan.ScanStatus.Ok);
    }
    
    public static Scan CreateExcess(string barcode, Item item, Box box, Order order, User user)
    {
        return Create(barcode, item, box, order, user, Scan.ScanStatus.Excess); 
    }
    
    public static Scan CreateNotFound(string barcode, Item item, Box box, Order order, User user)
    {
        return Create(barcode, item, box, order, user, Scan.ScanStatus.NotFound);
    }
    
    public static Scan CreateError(string barcode, Item item, Box box, Order order, User user)
    {
        return Create(barcode, item, box, order, user, Scan.ScanStatus.Error);
    }
}