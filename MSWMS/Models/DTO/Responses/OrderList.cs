namespace MSWMS.Models.Responses;

public class OrderList
{
    public required List<OrderDto> Orders { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
}