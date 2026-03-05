using MSWMS.Entities.Distributions;
using MSWMS.Models.DTO.Soap.Responses;
using MSWMS.Repositories;
using MSWMS.Services.Soap;
using MSWMS.TempModels;

namespace MSWMS.Services;

public class DcxDistributionService
{
    private readonly DcxDistributionRepository _dcxDistributionRepository;
    private readonly DcxSoapService _soapService;
    private readonly LocationRepository _locationRepository;
    
    public DcxDistributionService(
        DcxDistributionRepository dcxDistributionRepository, 
        DcxSoapService soapService,
        LocationRepository locationRepository
        )
    {
        _dcxDistributionRepository = dcxDistributionRepository;
        _soapService = soapService;
        _locationRepository = locationRepository;
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

    private async Task<List<DistributionItem>> LinesToDistributionItems(DcxMsWarehouseActivityLine[] lines)
    {
        var distributionItems = new List<DistributionItem>();
        
        foreach (var line in lines)
        {
            var locationCode = await GetLocationCodeByOrderNumber(line.SourceNo);
            var location = await _locationRepository.GetByCode(locationCode);
            
            if (location == null)
            {
                throw new InvalidOperationException($"Location not found for code: {locationCode}");
            }
            
            var distributionItem = new DistributionItem
            {
                ItemNumber = line.ItemNo,
                Variant = string.IsNullOrWhiteSpace(line.VariantCode) ? null : line.VariantCode,
                Description = line.Description,
                Quantity = (int)line.Quantity,
                BinCode = line.BinCode,
                LotNumber = line.LotNo,
                Destination = location,
                DocumentId = 0,
                Document = null!
            };
            
            distributionItems.Add(distributionItem);
        }
        
        return distributionItems;
    }

    private async Task<string> GetLocationCodeByOrderNumber(string orderNumber)
    {
        return await _dcxDistributionRepository.GetTransferDestinationLocationCode(orderNumber);
    }
}