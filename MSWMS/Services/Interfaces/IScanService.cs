using MSWMS.Entities;

namespace MSWMS.Services.Interfaces;

public interface IScanService
{
    /*public void DeleteScan(int id);

    public void CreateScan(Scan scan);

    public Scan GetScan(int id);

    public void UpdateScan(Scan scan);*/

    public Task<Scan.ScanStatus> ProcessScan(string barcode, int orderId, int boxNumber, int userId);
    
    public Task<Item?> GetItemByBarcodeAndOrder(string barcode, int orderId);
    
    public void AddScanToOrder(Scan scan, int orderId);
    
    public void DeleteScanFromOrder(Scan scan, int orderId);
    
    /*
    public List<Scan> GetScans();
    
    public List<Scan> GetScansByOrder(int orderId);*/
}