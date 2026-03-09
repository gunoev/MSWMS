using MSWMS.Entities.Distributions;
using MSWMS.Models.DTO.Soap.Responses;
using MSWMS.TempModels;

namespace MSWMS.Services.Interfaces;

public interface IDcxDistributionService
{
    Task<List<DirectedPickGetHeadersResult>> GetDirectedPickHeadersAsync(string locationCode);
    Task<List<DcxMsWarehouseActivityLine>> GetDirectedPickLinesAsync(string documentNumber);

    Task<List<DistributionItem>> LinesToDistributionItems(List<DcxMsWarehouseActivityLine> lines);
}
