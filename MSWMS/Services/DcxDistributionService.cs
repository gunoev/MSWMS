using MSWMS.Models.DTO.Soap.Responses;
using MSWMS.Repositories;
using MSWMS.Services.Soap;
using MSWMS.TempModels;

namespace MSWMS.Services;

public class DcxDistributionService
{
    private readonly DcxDistributionRepository _dcxDistributionRepository;
    private readonly DcxSoapService _soapService;
    
    public DcxDistributionService(
        DcxDistributionRepository dcxDistributionRepository, 
        DcxSoapService soapService
        )
    {
        _dcxDistributionRepository = dcxDistributionRepository;
        _soapService = soapService;
    }

    public async Task<List<DirectedPickGetHeadersResult>> GetDirectedPickHeadersAsync(string locationCode)
    {
        var headers = await _soapService.GetDirectedPickHeaders(locationCode);
        
        return headers;
    }

    public async Task<List<DcxMsWarehouseActivityLine>> GetDirectedPickLinesAsync(string documentNumber)
    {
        var lines = await _dcxDistributionRepository.GetLinesByDocumentNumber(documentNumber);
        
        return lines;
    }
}