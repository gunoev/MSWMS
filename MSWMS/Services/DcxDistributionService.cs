using MSWMS.Entities.Distributions;
using MSWMS.Models;
using MSWMS.Models.DTO.Soap.Responses;
using MSWMS.Repositories.Interfaces;
using MSWMS.Services.Interfaces;
using MSWMS.TempModels;

namespace MSWMS.Services;

public class DcxDistributionService : IDcxDistributionService
{
    private readonly IDcxDistributionRepository _dcxDistributionRepository;
    private readonly IDcxSoapService _soapService;
    private readonly ILocationRepository _locationRepository;
    
    public DcxDistributionService(
        IDcxDistributionRepository dcxDistributionRepository, 
        IDcxSoapService soapService,
        ILocationRepository locationRepository
        )
    {
        _dcxDistributionRepository = dcxDistributionRepository;
        _soapService = soapService;
        _locationRepository = locationRepository;
    }

    public async Task<List<DirectedPickGetHeadersResult>> GetDirectedPickHeadersAsync(string locationCode)
    {
        var headers = await _soapService.GetDirectedPickHeadersAsync(locationCode);
        
        return headers;
    }

    public async Task<List<DcxMsWarehouseActivityLine>> GetDirectedPickLinesAsync(string documentNumber)
    {
        var lines = await _dcxDistributionRepository.GetLinesByDocumentNumber(documentNumber);
        
        return lines;
    }

    public async Task<List<DistributionItem>> LinesToDistributionItems(List<DcxMsWarehouseActivityLine> lines)
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
                DocumentId = 0,
                DestinationId = location.Id,    
                Document = null!
            };
            
            distributionItems.Add(distributionItem);
        }
        
        return distributionItems;
    }

    public async Task<DcxMsItemCrossReference?> GetItemCrossReference(string barcode)
    {
        return await _dcxDistributionRepository.GetItemCrossReference(barcode);
    }

    public async Task<string> GetLocationCodeByOrderNumber(string orderNumber)
    {
        if (orderNumber.Contains("SO"))
        {
            return await _dcxDistributionRepository.GetSalesDestinationLocationCode(orderNumber);
        }
        return await _dcxDistributionRepository.GetTransferDestinationLocationCode(orderNumber);
    }
}
