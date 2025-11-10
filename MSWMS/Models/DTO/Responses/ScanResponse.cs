using MSWMS.Entities;

namespace MSWMS.Models.Responses;
public class ScanResponse
{
    public required ScanDto Scan { get; set; }
    public ItemDto? Item { get; set; }
    public required BoxDto Box { get; set; }
    public string Username { get; set; }
}