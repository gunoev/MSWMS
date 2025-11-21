namespace MSWMS.Models.Responses;

public class BoxDto
{
    public int Id { get; set; }
    public string Guid { get; set; }
    public int BoxNumber { get; set; }
    public int Quantity { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public bool HasShipmentEvents { get; set; }
}