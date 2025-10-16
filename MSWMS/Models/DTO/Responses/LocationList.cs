namespace MSWMS.Models.Responses;

public class LocationList
{
    public required List<LocationDto> Locations { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
}