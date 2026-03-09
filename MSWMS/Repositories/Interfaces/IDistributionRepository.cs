using MSWMS.Entities.Distributions;

namespace MSWMS.Repositories.Interfaces;

public interface IDistributionRepository
{
    Task<Distribution?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Distribution>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Distribution> AddAsync(Distribution entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(Distribution entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<ICollection<Distribution>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate);


}