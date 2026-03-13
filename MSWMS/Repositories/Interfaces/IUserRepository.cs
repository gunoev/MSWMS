using MSWMS.Entities;

namespace MSWMS.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByNameAsync(string userName);
}