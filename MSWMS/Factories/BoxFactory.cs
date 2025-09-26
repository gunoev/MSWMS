using MSWMS.Entities;

namespace MSWMS.Factories;

public class BoxFactory
{
    public static Box Create(int boxNumber, Order order, User user, string? uniqueId = null)
    {
        return new Box
        {
            BoxNumber = boxNumber,
            Order = order,
            User = user,
            UniqueId = uniqueId
        };
    }
}