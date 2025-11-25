using System.ComponentModel.DataAnnotations;

namespace MSWMS.Entities;

public class Location
{
    public int Id { get; set; }
    [MaxLength(256)]
    public required string Name { get; set; }
    [MaxLength(64)]
    public string? ShortName { get; set; }
    [MaxLength(16)]
    public required string Code { get; set; }
}