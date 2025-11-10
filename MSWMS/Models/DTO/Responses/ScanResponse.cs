using MSWMS.Models.Responses;

namespace MSWMS.Models.DTO.Responses;
public class ScanResponse
{
    public required ScanDto Scan { get; set; }
    public ItemDto? Item { get; set; }
    public required BoxDto Box { get; set; }
}