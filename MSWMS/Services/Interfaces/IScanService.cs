using MSWMS.Entities;

namespace MSWMS.Services.Interfaces;

public interface IScanService
{
    /*public void DeleteScan(int id);

    public void CreateScan(Scan scan);

    public Scan GetScan(int id);

    public void UpdateScan(Scan scan);*/

    public Scan.ScanStatus ProcessBarcode(string barcode);
    
    public Item? GetItemByBarcode(string barcode);
    
    /*
    public List<Scan> GetScans();
    
    public List<Scan> GetScansByOrder(int orderId);*/
}