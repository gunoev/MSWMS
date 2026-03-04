using MSWMS.Entities.Distributions;

namespace MSWMS.Services.Interfaces;

public interface IDistributionService
{
    Task<Distribution?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Distribution>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Distribution> CreateAsync(Distribution distribution, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Distribution distribution, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<DistributionDocument?> AddDocumentAsync(
        int distributionId,
        string documentNumber,
        string orderNumber,
        int originId,
        int destinationId,
        CancellationToken cancellationToken = default);
    Task<bool> RemoveDocumentAsync(int distributionId, int documentId, CancellationToken cancellationToken = default);
    Task<Distribution?> UpdateNoteAsync(int distributionId, string note, CancellationToken cancellationToken = default);
}
