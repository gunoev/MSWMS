using MSWMS.Entities;
using MSWMS.Models.DTO.Responses;
using MSWMS.Models.Requests;
using MSWMS.Models.Responses;

namespace MSWMS.Services.Interfaces;

public interface IScanService
{
    /*public void DeleteScan(int id);

    public void CreateScan(Scan scan);

    public Scan GetScan(int id);

    public void UpdateScan(Scan scan);*/

    public Task<ScanResponse?> ProcessScan(ScanRequest request);
    
    public Task<Item?> GetItemByBarcodeAndOrder(string barcode, int orderId);
    
    public Task AddScanToOrder(Scan scan, Order order);
    
    public void DeleteScanFromOrder(Scan scan, Order order);

    public Task<int> GetScannedQuantity(Item item, Order order);

    /*
    public List<Scan> GetScans();

    public List<Scan> GetScansByOrder(int orderId);*/
}