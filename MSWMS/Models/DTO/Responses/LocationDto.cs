using MSWMS.Entities;

namespace MSWMS.Models.Responses;

public class LocationDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string ShortName { get; set; }
    public required string Code { get; set; }

    public LocationDto FromEntity(Location location)
    {
        Id = location.Id;
        Name = location.Name;
        ShortName = location.ShortName;
        Code = location.Code;

        return this;
    }
}