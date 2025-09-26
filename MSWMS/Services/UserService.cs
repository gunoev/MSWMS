using MSWMS.Entities;

namespace MSWMS.Services;

public class UserService
{
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        await using var db = new AppDbContext();
        
        return db.Users.FirstOrDefault(u => u.Id == userId);
    }
}