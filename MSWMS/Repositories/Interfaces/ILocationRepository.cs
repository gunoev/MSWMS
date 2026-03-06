using MSWMS.Entities;

namespace MSWMS.Repositories.Interfaces;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Location?> GetByCode(string code, CancellationToken cancellationToken = default);
}