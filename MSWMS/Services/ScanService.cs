using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Factories;
using MSWMS.Models.Requests;
using MSWMS.Models.Responses;
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
    public async Task<ScanResponse> ProcessScan(ScanRequest request)
    {
        var startTime = DateTime.Now;
        var item = await GetItemByBarcodeAndOrder(request.Barcode, request.OrderId);
        Console.WriteLine($"Time to get item: {DateTime.Now - startTime}");
        var order = await _context.Orders
            .Include(o => o.Boxes)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId);//await _orderService.GetByIdAsync(request.OrderId);
        Console.WriteLine($"Time to get order: {DateTime.Now - startTime}");
        var box = await _boxService.GetBoxByNumberAndOrder(request.BoxNumber, request.OrderId);
        Console.WriteLine($"Time to get box: {DateTime.Now - startTime}");
        var user = await _userService.GetUserByIdAsync(request.UserId);
        Console.WriteLine($"Time to get user: {DateTime.Now - startTime}");


        if (order is null)
        {
            return new ScanResponse
            {
                Id = 0,
                Barcode = request.Barcode, 
                TimeStamp = DateTime.Now,
                Status = Scan.ScanStatus.Error,
                BoxNumber = request.BoxNumber,
                BoxId = 0,
                ItemId = 0,
                OrderId = 0,
                Username = user.Username,
            };
        }
        if (box is null || box.User.Username != user?.Username) // create new box increment box number
        {
            box = BoxFactory.Create(order.Boxes?.Any() == true ? order.Boxes.Max(b => b.BoxNumber) + 1 : 1, order, user);
        }

        Scan scan;

        if (item == null)
        {
            scan = ScanFactory.CreateNotFound(request.Barcode, item, box, order, user);
            await AddScanToOrder(scan, order);
            await _context.SaveChangesAsync();
            return new ScanResponse
            {
                Id = scan.Id,
                Barcode = scan.Barcode, 
                TimeStamp = DateTime.Now,
                Status = Scan.ScanStatus.NotFound,
                BoxId = box.Id,
                ItemId = 0,
                OrderId = order.Id,
                BoxNumber = box.BoxNumber,
                Username = user.Username,
            };
        }
        if (GetScannedQuantity(item, order).Result < item.NeededQuantity)
        {
            scan = ScanFactory.CreateOk(request.Barcode, item, box, order, user);
            await AddScanToOrder(scan, order);
            await _context.SaveChangesAsync();
            return new ScanResponse
            {
                Id = scan.Id,
                Barcode = scan.Barcode, 
                TimeStamp = DateTime.Now,
                Status = Scan.ScanStatus.Ok,
                BoxNumber = box.BoxNumber,
                BoxId = box.Id,
                ItemId = item.Id,
                OrderId = order.Id,
                Username = user.Username,
            };
        }
        if (item != null &&  (GetScannedQuantity(item, order).Result >= item.NeededQuantity))
        {
            scan = ScanFactory.CreateExcess(request.Barcode, item, box, order, user);
            await AddScanToOrder(scan, order);
            await _context.SaveChangesAsync();
            return new ScanResponse
            {
                Id = scan.Id,
                Barcode = scan.Barcode, 
                TimeStamp = DateTime.Now,
                Status = Scan.ScanStatus.Excess,
                BoxNumber = box.BoxNumber,
                BoxId = box.Id,
                ItemId = item.Id,
                OrderId = order.Id,
                Username = user.Username,
            };
        }
        
        scan = ScanFactory.CreateError(request.Barcode, item, box, order, user);
        await AddScanToOrder(scan, order);
        await _context.SaveChangesAsync();
        return new ScanResponse
        {
            Id = scan.Id,
            Barcode = scan.Barcode,
            TimeStamp = DateTime.Now,
            Status = Scan.ScanStatus.Error,
            BoxNumber = box.BoxNumber,
            BoxId = box.Id,
            ItemId = item.Id,
            OrderId = order.Id,
            Username = user.Username
        };
        
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
        
            order.Scans.Add(scan);
        
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            await UpdateOrderStatus(order);
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    private async Task UpdateOrderStatus(Order order)
    {
        var currentStatus = order.Status;
    
        var totalNeededQuantity = order.Items.Sum(i => i.NeededQuantity);
        var totalScannedQuantity = await _context.Scans.CountAsync(s => s.Order.Id == order.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess));
    
        if (totalScannedQuantity == 0 && currentStatus != Order.OrderStatus.New)
        {
            order.Status = Order.OrderStatus.New;
        }
        else if (totalScannedQuantity < totalNeededQuantity && currentStatus != Order.OrderStatus.InProgress)
        {
            order.Status = Order.OrderStatus.InProgress; 
        }
        else if (totalScannedQuantity >= totalNeededQuantity && currentStatus != Order.OrderStatus.Collected)
        {
            order.Status = Order.OrderStatus.Collected;
            order.CollectedDateTime = DateTime.Now;
        }
    
        if (currentStatus != order.Status)
        {
            order.LastChangeDateTime = DateTime.Now;
            await _context.SaveChangesAsync();
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