using MSWMS.Entities;
using MSWMS.Factories;
using MSWMS.Models.Requests;
using MSWMS.Services.Interfaces;

namespace MSWMS.Services;

public class ScanService : IScanService
{
    private readonly BoxService _boxService =  new BoxService();
    private readonly OrderService _orderService =  new OrderService();
    private readonly UserService _userService =  new UserService();
    public async Task<Scan.ScanStatus> ProcessScan(ScanRequest request)
    {
        var item = GetItemByBarcodeAndOrder(request.Barcode, request.OrderId).Result;
        var order = _orderService.GetByIdAsync(request.OrderId).Result;
        var box = _boxService.GetBoxByNumberAndOrder(request.BoxNumber, request.OrderId).Result;
        var user = _userService.GetUserByIdAsync(request.UserId).Result;

        if (order is null)
        {
            return Scan.ScanStatus.Error;
        }
        if (box is null || box.User.Username != user?.Username) // create new box increment box number
        {
            box = BoxFactory.Create(request.BoxNumber, order, user);
        }

        if (item == null)
        {
            var scan = ScanFactory.CreateNotFound(request.Barcode, item, box, order, user);
            AddScanToOrder(scan, request.OrderId);
            return Scan.ScanStatus.NotFound;
        }
        else if (item != null && (GetScannedQuantity(item, order).Result < item.NeededQuantity))
        {
            var scan =ScanFactory.CreateOk(request.Barcode, item, box, order, user);
            AddScanToOrder(scan, request.OrderId);
            return Scan.ScanStatus.Ok;
        }
        else if (item != null &&  (GetScannedQuantity(item, order).Result >= item.NeededQuantity))
        {
            var scan = ScanFactory.CreateExcess(request.Barcode, item, box, order, user);
            AddScanToOrder(scan, request.OrderId);
            return Scan.ScanStatus.Excess;
        }
        else
        {
            var scan = ScanFactory.CreateError(request.Barcode, item??null, box, order, user);
            AddScanToOrder(scan, request.OrderId);
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