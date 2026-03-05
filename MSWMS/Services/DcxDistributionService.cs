using MSWMS.Models.DTO.Soap.Responses;
using MSWMS.Repositories;
using MSWMS.Services.Soap;

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

    public async Task<List<DirectedPickGetHeadersResult>> GetDirectedPickHeaders(string locationCode)
    {
        var headers = await _soapService.GetDirectedPickHeaders(locationCode);
        
        return headers;
    }
}