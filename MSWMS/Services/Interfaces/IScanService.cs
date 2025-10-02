using MSWMS.Entities;
using MSWMS.Models.Requests;

namespace MSWMS.Services.Interfaces;

public interface IScanService
{
    /*public void DeleteScan(int id);

    public void CreateScan(Scan scan);

    public Scan GetScan(int id);

    public void UpdateScan(Scan scan);*/

    public Task<Scan.ScanStatus> ProcessScan(ScanRequest request);
    
    public Task<Item?> GetItemByBarcodeAndOrder(string barcode, int orderId);
    
    public Task AddScanToOrder(Scan scan, Order order);
    
    public void DeleteScanFromOrder(Scan scan, Order order);
    
    /*
    public List<Scan> GetScans();
    
    public List<Scan> GetScansByOrder(int orderId);*/
}