using MSWMS.Entities;
using MSWMS.Services.Interfaces;

namespace MSWMS.Services;

public class ScanService : IScanService
{
    public Task<Scan.ScanStatus> ProcessBarcode(string barcode)
    {
        throw new NotImplementedException();
    }

    public async Task<Item?> GetItemByBarcodeAndOrder(string barcode, int orderId)
    {
        await using var db = new AppDbContext();
        Item? item = db.Items.FirstOrDefault(i =>
            i.Order.Id == orderId && i.ItemInfo.Any(info => info.Barcode == barcode));

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