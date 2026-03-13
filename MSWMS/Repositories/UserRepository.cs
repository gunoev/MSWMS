using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Repositories.Interfaces;

namespace MSWMS.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public Task<User?> GetByNameAsync(string userName)
    {
        return context.Users.FirstOrDefaultAsync(u => u.Username == userName);
    }
}