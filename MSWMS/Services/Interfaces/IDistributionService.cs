using MSWMS.Entities.Distributions;
using MSWMS.Models.DTO.Requests;
using MSWMS.Models.DTO.Responses.Distributions;
using MSWMS.Models.DTO.Soap.Responses;

namespace MSWMS.Services.Interfaces;

public interface IDistributionService
{
    Task<Distribution?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Distribution>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Distribution> CreateAsync(Distribution distribution, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Distribution distribution, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<DistributionDocument?> AddDocumentAsync(
        CreateDistributionDocumentRequest request,
        CancellationToken cancellationToken = default);
    Task<bool> RemoveDocumentAsync(int distributionId, int documentId, CancellationToken cancellationToken = default);
    Task<Distribution?> UpdateNoteAsync(int distributionId, string note, CancellationToken cancellationToken = default);

    Task<DistributionDto?> GetDistributionDtoByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DistributionDocumentDto>> GetDocumentsDtoAsync(int distributionId, CancellationToken cancellationToken = default);
    Task<DistributionDocumentDto?> GetDocumentDtoByIdAsync(int distributionId, int documentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DistributionItemDto>> GetDistributionItemsDtoAsync(int distributionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DistributionScanDto>> GetScansDtoAsync(int distributionId, CancellationToken cancellationToken = default);
    Task<DistributionScanDto?> GetScanDtoByIdAsync(int distributionId, int scanId, CancellationToken cancellationToken = default);

    Task<Distribution> CreateDistributionAsync(CreateDistributionRequest request);

    Task<List<DistributionDocument>> AddDocumentsAsync(
        List<CreateDistributionDocumentRequest> documentRequests, CancellationToken cancellationToken = default);

    Task<DistributionScanDto> ProceedScanAsync(DistributionScanRequest request);
    
    Task<ICollection<Distribution>> GetDistributionsByDateRangeAsync(DateOnly startDate, DateOnly endDate);

    Task<List<DirectedPickGetHeadersResult>> GetDirectedPickHeaders();

    Task<ICollection<DistributionDto>> GetDistributionsDtoByDateRangeAsync(DateOnly startDate, DateOnly endDate);
}
