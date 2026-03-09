using MSWMS.Entities.Distributions;

namespace MSWMS.Repositories.Interfaces;

public interface IDistributionDocumentRepository
{
    Task<DistributionDocument?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<DistributionDocument?> GetByDistributionAndDocumentIdAsync(
        int distributionId,
        int documentId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<DistributionDocument>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<DistributionDocument> AddAsync(DistributionDocument entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(DistributionDocument entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
