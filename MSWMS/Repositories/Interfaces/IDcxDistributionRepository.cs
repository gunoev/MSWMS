using MSWMS.TempModels;

namespace MSWMS.Repositories.Interfaces;

public interface IDcxDistributionRepository
{
    Task<List<DcxMsWarehouseActivityLine>> GetLinesByDocumentNumber(string documentNumber,
        int activityType = 2, int actionType = 1, CancellationToken cancellationToken = default);
    
    Task<string> GetTransferDestinationLocationCode(string documentNumber, CancellationToken cancellationToken = default);

    Task<string> GetSalesDestinationLocationCode(string documentNumber);
}