using MSWMS.Entities;

namespace MSWMS.Models.Responses;

public class ScanResponse
{
    public int Id { get; set; }
    public required string Barcode { get; set; }
    public int BoxNumber { get; set; }
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required DateTime TimeStamp { get; set; }
    public Scan.ScanStatus Status { get; set; }
    
}