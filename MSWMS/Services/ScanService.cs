using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Factories;
using MSWMS.Models.Requests;
using MSWMS.Services.Interfaces;

namespace MSWMS.Services;

public class ScanService : IScanService
{
    private readonly BoxService _boxService;
    private readonly OrderService _orderService;
    private readonly UserService _userService;
    private readonly AppDbContext _context;
    
    public ScanService(OrderService orderService, BoxService boxService, UserService userService, AppDbContext context)
    {
        _context = context;
        _orderService = orderService;
        _boxService = boxService;
        _userService = userService;
    }
    public async Task<Scan.ScanStatus> ProcessScan(ScanRequest request)
    {
        var item = await GetItemByBarcodeAndOrder(request.Barcode, request.OrderId);
        var order = await _orderService.GetByIdAsync(request.OrderId);
        var box = await _boxService.GetBoxByNumberAndOrder(request.BoxNumber, request.OrderId);
        var user = await _userService.GetUserByIdAsync(request.UserId);

        if (order is null)
        {
            return Scan.ScanStatus.Error;
        }
        if (box is null || box.User.Username != user?.Username) // create new box increment box number
        {
            box = BoxFactory.Create(request.BoxNumber, order, user);
        }

        Scan scan;

        if (item == null)
        {
            scan = ScanFactory.CreateNotFound(request.Barcode, item, box, order, user);
            await AddScanToOrder(scan, order);
            await _context.SaveChangesAsync();
            return Scan.ScanStatus.NotFound;
        }
        if (GetScannedQuantity(item, order).Result < item.NeededQuantity)
        {
            scan = ScanFactory.CreateOk(request.Barcode, item, box, order, user);
            await AddScanToOrder(scan, order);
            await _context.SaveChangesAsync();
            return Scan.ScanStatus.Ok;
        }
        if (item != null &&  (GetScannedQuantity(item, order).Result >= item.NeededQuantity))
        {
            scan = ScanFactory.CreateExcess(request.Barcode, item, box, order, user);
            await AddScanToOrder(scan, order);
            await _context.SaveChangesAsync();
            return Scan.ScanStatus.Excess;
        }
        
        scan = ScanFactory.CreateError(request.Barcode, item, box, order, user);
        await AddScanToOrder(scan, order);
        await _context.SaveChangesAsync();
        return Scan.ScanStatus.Error;
        
    }

    public async Task<int> GetScannedQuantity(Item item, Order order)
    {
        return await _context.Scans.CountAsync(s => s.Item.Id == item.Id && s.Order.Id == order.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess));
    }

    public async Task<Item?> GetItemByBarcodeAndOrder(string barcode, int orderId)
    {
        Item? item = await _context.Orders
            .Where(o => o.Id == orderId)
            .SelectMany(o => o.Items)
            .FirstOrDefaultAsync(i => i.ItemInfo.Any(iif => iif.Barcode == barcode));

        return item;
    }
    
    
    public async Task AddScanToOrder(Scan scan, Order order)
    {
        try
        {
            if (order.Scans is null)
            {
                order.Scans = new List<Scan>();
            }
            
            /*_context.Scans.Add(scan);*/
        
            order.Scans.Add(scan);
        
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    public async void DeleteScanFromOrder(Scan scan, Order order)
    {
        try
        {
            order.Scans.Remove(scan);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
    
    
}