namespace MSWMS.Entities;

public class Scan
{
    public int Id { get; set; }
    public required string Barcode { get; set; }
    public DateTime TimeStamp { get; set; }
    public required ScanStatus Status { get; set; }
    
    public required Item Item { get; set; }
    public required Box Box { get; set; }
    public required Order Order { get; set; }
    public required User User { get; set; }

    public enum ScanStatus
    {
        Ok,
        Excess,
        NotFound,
        Error,
    }
}