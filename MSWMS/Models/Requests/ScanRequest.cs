namespace MSWMS.Models.Requests;

public class ScanRequest
{
    public int Id { get; set; }
    public required string Barcode { get; set; }
    public required int BoxNumber { get; set; }
    public required int OrderId { get; set; }
}