using MSWMS.Entities;
using MSWMS.Factories;
using MSWMS.Services.Interfaces;

namespace MSWMS.Services;

public class ScanService : IScanService
{
    private readonly BoxService _boxService =  new BoxService();
    private readonly OrderService _orderService =  new OrderService();
    private readonly UserService _userService =  new UserService();
    public async Task<Scan.ScanStatus> ProcessScan(string barcode, int orderId, int boxNumber, int userId)
    {
        var item = GetItemByBarcodeAndOrder(barcode, orderId).Result;
        var order = _orderService.GetByIdAsync(orderId).Result;
        var box = _boxService.GetBoxByNumberAndOrder(boxNumber, orderId).Result;
        var user = _userService.GetUserByIdAsync(userId).Result;
        if (box is null || box.User.Username != user?.Username)
        {
            box = BoxFactory.Create(boxNumber, order, user);
        }

        if (item == null)
        {
            var scan = ScanFactory.CreateNotFound(barcode, item, box, order, user);
            AddScanToOrder(scan, orderId);
            return Scan.ScanStatus.NotFound;
        }
        else if (item != null && (GetScannedQuantity(item, order).Result < item.NeededQuantity))
        {
            var scan =ScanFactory.CreateOk(barcode, item, box, order, user);
            AddScanToOrder(scan, orderId);
            return Scan.ScanStatus.Ok;
        }
        else if (item != null &&  (GetScannedQuantity(item, order).Result >= item.NeededQuantity))
        {
            var scan = ScanFactory.CreateExcess(barcode, item, box, order, user);
            AddScanToOrder(scan, orderId);
            return Scan.ScanStatus.Excess;
        }
        else
        {
            var scan = ScanFactory.CreateError(barcode, item??null, box, order, user);
            AddScanToOrder(scan, orderId);
            return Scan.ScanStatus.Error;
        }
    }

    public async Task<int> GetScannedQuantity(Item item, Order order)
    {
        await using var db = new AppDbContext();
        return db.Scans.Count(s => s.Item.Id == item.Id && s.Order.Id == order.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess));
    }

    public async Task<Item?> GetItemByBarcodeAndOrder(string barcode, int orderId)
    {
        await using var db = new AppDbContext();
        Item? item = db.Orders
            .Where(o => o.Id == orderId)
            .SelectMany(o => o.Items)
            .FirstOrDefault(i => i.ItemInfo.Any(iif => iif.Barcode == barcode));

        return item;
    }
    
    public async void AddScanToOrder(Scan scan, int orderId)
    {
        try
        {
            await using var db = new AppDbContext();
            var order = db.Orders.FindAsync(orderId).Result;
            order?.Scans.Add(scan);
            await db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    public async void DeleteScanFromOrder(Scan scan, int orderId)
    {
        try
        {
            await using var db = new AppDbContext();
            var order = db.Orders.FindAsync(orderId).Result;
            order?.Scans.Remove(scan);
            await db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
    
    
}