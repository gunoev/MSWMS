using MSWMS.Entities.Distributions;
using MSWMS.Infrastructure.Helpers;
using MSWMS.Interfaces;
using MSWMS.Models;
using MSWMS.Models.DTO.Requests;
using MSWMS.Models.DTO.Responses.Distributions;
using MSWMS.Models.DTO.Soap.Responses;
using MSWMS.Repositories.Interfaces;
using MSWMS.Services.Interfaces;
using Serilog;

namespace MSWMS.Services;

public class DistributionService : IDistributionService
{
    private readonly IDistributionRepository _distributionRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IDistributionDocumentRepository _distributionDocumentRepository;
    private readonly IDcxDistributionService _dcxDistributionService;
    private readonly IAsyncRepository<DistributionScan> _distributionScanRepository;

    public DistributionService(IDistributionRepository distributionRepository, ILocationRepository locationRepository,
        IDistributionDocumentRepository distributionDocumentRepository, IDcxDistributionService dcxDistributionService,
        IAsyncRepository<DistributionScan> distributionScanRepository)
    {
        _distributionRepository = distributionRepository;
        _locationRepository = locationRepository;
        _distributionDocumentRepository = distributionDocumentRepository;
        _dcxDistributionService = dcxDistributionService;
        _distributionScanRepository = distributionScanRepository;
    }

    public Task<Distribution?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _distributionRepository.GetByIdAsync(id, cancellationToken);
    }

    public Task<Distribution?> GetDistributionByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetByIdAsync(id, cancellationToken);
    }

    public Task<IEnumerable<Distribution>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _distributionRepository.GetAllAsync(cancellationToken);
    }

    public Task<Distribution> CreateAsync(Distribution distribution, CancellationToken cancellationToken = default)
    {
        return _distributionRepository.AddAsync(distribution, cancellationToken);
    }

    public async Task<bool> UpdateAsync(Distribution distribution, CancellationToken cancellationToken = default)
    {
        var existingDistribution = await _distributionRepository.GetByIdAsync(distribution.Id, cancellationToken);
        if (existingDistribution is null)
        {
            return false;
        }

        existingDistribution.Date = distribution.Date;
        existingDistribution.Note = distribution.Note;

        await _distributionRepository.UpdateAsync(existingDistribution, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var distribution = await _distributionRepository.GetByIdAsync(id, cancellationToken);
        if (distribution is null)
        {
            return false;
        }

        await _distributionRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    public Task<bool> DeleteDistributionAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(id, cancellationToken);
    }

    public async Task<DistributionDocument?> CreateDocumentAsync(CreateDistributionDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var distribution = await _distributionRepository.GetByIdAsync(request.DistributionId, cancellationToken);

        if (distribution is null)
        {
            return null;
        }

        var origin = await _locationRepository.GetByIdAsync(request.OriginId, cancellationToken);
        var destination = await _locationRepository.GetByIdAsync(request.DestinationId, cancellationToken);

        if (origin is null)
        {
            throw new Exception("Origin location not found.");
        }

        var lines = await _dcxDistributionService.GetDirectedPickLinesAsync(request.DocumentNumber);
        var items = await _dcxDistributionService.LinesToDistributionItems(lines);

        var document = new DistributionDocument
        {
            DocumentNumber = request.DocumentNumber,
            OrderNumber = request.OrderNumber,
            OriginId = origin.Id,
            DistributionId = distribution.Id,
            Items = items
        };

        return document;
    }

    public async Task<DistributionDocument?> AddDocumentAsync(CreateDistributionDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var document = await CreateDocumentAsync(request, cancellationToken);

        if (document is null)
        {
            return null;
        }

        await _distributionDocumentRepository.AddAsync(document, cancellationToken);

        return document;
    }

    public async Task<List<DistributionDocument>> AddDocumentsAsync(
        List<CreateDistributionDocumentRequest> documentRequests, CancellationToken cancellationToken = default)
    {
        var documents = new List<DistributionDocument>();

        foreach (var request in documentRequests)
        {
            var document = await CreateDocumentAsync(request, cancellationToken);

            if (document is null)
            {
                continue;
            }

            await _distributionDocumentRepository.AddAsync(document, cancellationToken);
            documents.Add(document);
        }

        return documents;
    }

    public async Task<IReadOnlyList<DistributionDocument>> GetDocumentsAsync(int distributionId,
        CancellationToken cancellationToken = default)
    {
        var distribution = await _distributionRepository.GetByIdAsync(distributionId, cancellationToken);
        if (distribution is null)
        {
            return [];
        }

        var documents = await _distributionDocumentRepository.GetAllAsync(cancellationToken);
        return documents.Where(d => d.DistributionId == distributionId).ToList();
    }

    public async Task<DistributionDocument?> GetDocumentByIdAsync(int distributionId, int documentId,
        CancellationToken cancellationToken = default)
    {
        var distribution = await _distributionRepository.GetByIdAsync(distributionId, cancellationToken);
        if (distribution is null)
        {
            return null;
        }

        return await _distributionDocumentRepository.GetByDistributionAndDocumentIdAsync(distributionId, documentId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<DistributionItem>> GetDistributionItemsAsync(int distributionId,
        CancellationToken cancellationToken = default)
    {
        var documents = await GetDocumentsAsync(distributionId, cancellationToken);
        return documents.SelectMany(d => d.Items).ToList();
    }

    public async Task<IReadOnlyList<DistributionScan>> GetScansAsync(int distributionId,
        CancellationToken cancellationToken = default)
    {
        var distribution = await _distributionRepository.GetByIdAsync(distributionId, cancellationToken);
        if (distribution is null)
        {
            return [];
        }

        return distribution.Scans.ToList();
    }

    public async Task<DistributionScan?> GetScanByIdAsync(int distributionId, int scanId,
        CancellationToken cancellationToken = default)
    {
        var scans = await GetScansAsync(distributionId, cancellationToken);
        return scans.FirstOrDefault(s => s.Id == scanId);
    }

    public async Task<DistributionDto?> GetDistributionDtoByIdAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var distribution = await GetDistributionByIdAsync(id, cancellationToken);
        if (distribution is null)
        {
            return null;
        }

        var documents = await GetDocumentsAsync(id, cancellationToken);
        var scans = await GetScansAsync(id, cancellationToken);

        return MapToDto(distribution, documents.Count, scans.Count);
    }

    public async Task<IReadOnlyList<DistributionDocumentDto>> GetDocumentsDtoAsync(int distributionId,
        CancellationToken cancellationToken = default)
    {
        var documents = await GetDocumentsAsync(distributionId, cancellationToken);
        return documents.Select(MapToDto).ToList();
    }

    public async Task<DistributionDocumentDto?> GetDocumentDtoByIdAsync(int distributionId, int documentId,
        CancellationToken cancellationToken = default)
    {
        var document = await GetDocumentByIdAsync(distributionId, documentId, cancellationToken);
        return document is null ? null : MapToDto(document);
    }

    public async Task<IReadOnlyList<DistributionItemDto>> GetDistributionItemsDtoAsync(int distributionId,
        CancellationToken cancellationToken = default)
    {
        var items = await GetDistributionItemsAsync(distributionId, cancellationToken);
        return items.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<DistributionScanDto>> GetScansDtoAsync(int distributionId,
        CancellationToken cancellationToken = default)
    {
        var scans = await GetScansAsync(distributionId, cancellationToken);
        return scans.Select(MapToDto).ToList();
    }

    public async Task<DistributionScanDto?> GetScanDtoByIdAsync(int distributionId, int scanId,
        CancellationToken cancellationToken = default)
    {
        var scan = await GetScanByIdAsync(distributionId, scanId, cancellationToken);
        return scan is null ? null : MapToDto(scan);
    }

    public async Task<bool> RemoveDocumentAsync(int distributionId, int documentId,
        CancellationToken cancellationToken = default)
    {
        var document =
            await _distributionDocumentRepository.GetByDistributionAndDocumentIdAsync(distributionId, documentId,
                cancellationToken);
        if (document is null)
        {
            return false;
        }

        await _distributionDocumentRepository.DeleteAsync(documentId, cancellationToken);
        return true;
    }

    public async Task<Distribution?> UpdateNoteAsync(int distributionId, string note,
        CancellationToken cancellationToken = default)
    {
        var distribution = await _distributionRepository.GetByIdAsync(distributionId, cancellationToken);
        if (distribution is null)
        {
            return null;
        }

        distribution.Note = note;
        await _distributionRepository.UpdateAsync(distribution, cancellationToken);
        return distribution;
    }

    public async Task<Distribution> CreateDistributionAsync(CreateDistributionRequest request)
    {
        var distribution = new Distribution { Date = request.Date, Note = request.Note };

        await _distributionRepository.AddAsync(distribution);

        return distribution;
    }

    public async Task<ICollection<Distribution>> GetDistributionsByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        return await _distributionRepository.GetByDateRangeAsync(startDate, endDate);
    }

    private static DistributionDto MapToDto(Distribution distribution, int documentsCount, int scansCount)
    {
        return new DistributionDto
        {
            Id = distribution.Id,
            Date = distribution.Date,
            Note = distribution.Note,
            DocumentsCount = documentsCount,
            ScansCount = scansCount
        };
    }

    private static DistributionDocumentDto MapToDto(DistributionDocument document)
    {
        return new DistributionDocumentDto
        {
            Id = document.Id,
            DocumentNumber = document.DocumentNumber,
            OrderNumber = document.OrderNumber,
            OriginId = document.OriginId,
            DestinationId = document.DestinationId,
            DistributionId = document.DistributionId,
            ItemsCount = document.Items.Count
        };
    }

    private static DistributionItemDto MapToDto(DistributionItem item)
    {
        return new DistributionItemDto
        {
            Id = item.Id,
            ItemNumber = item.ItemNumber,
            Variant = item.Variant,
            Description = item.Description,
            Quantity = item.Quantity,
            BinCode = item.BinCode,
            LotNumber = item.LotNumber,
            DocumentId = item.DocumentId
        };
    }

    private static DistributionScanDto MapToDto(DistributionScan scan)
    {
        return new DistributionScanDto
        {
            Id = scan.Id,
            Barcode = scan.Barcode,
            BinCode = scan.BinCode,
            LotNumber = scan.LotNumber,
            ScanType = scan.ScanType,
            TimeStamp = scan.TimeStamp,
            Status = scan.Status,
            UserId = scan.UserId,
            OriginId = scan.OriginId,
            DistributionId = scan.DistributionId,
            DocumentId = scan.DocumentId,
            ItemId = scan.ItemId
        };
    }

    public async Task<DistributionScanDto> ProceedScanAsync(DistributionScanRequest request)
    {
        var distribution = await _distributionRepository.GetByIdAsync(request.DistributionId);
        if (distribution is null)
        {
            return MapToDto(CreateTransientScan(request, DistributionScanStatus.Error));
        }

        var document = await GetDocumentWithItemsAsync(request.DistributionId, request.DocumentId);
        if (document is null)
        {
            return MapToDto(CreateTransientScan(request, DistributionScanStatus.Error, distributionId: distribution.Id));
        }

        var item = FindMatchingItem(document, request.Item);
        var crossReference = await _dcxDistributionService.GetItemCrossReference(request.Barcode);
        var status = await ResolveScanStatusAsync(request, item, crossReference);

        if (status != DistributionScanStatus.Ok)
        {
            return MapToDto(CreateTransientScan(request, status, distributionId: distribution.Id,
                documentId: document.Id, itemId: item?.Id));
        }

        var scan = CreateScan(request, distribution.Id, document, item!, DistributionScanStatus.Ok);
        await _distributionScanRepository.AddAsync(scan);

        return MapToDto(scan);
    }

    private async Task<DistributionDocument?> GetDocumentWithItemsAsync(int distributionId, int documentId)
    {
        var document = await _distributionDocumentRepository.GetByIdAsync(documentId);
        if (document is null || document.DistributionId != distributionId)
        {
            return null;
        }

        return document;
    }

    private static DistributionItem? FindMatchingItem(DistributionDocument document, DistributionItemDto itemDto)
    {
        if (itemDto.Id != 0)
        {
            return document.Items.FirstOrDefault(i => i.Id == itemDto.Id);
        }

        var targetVariant = StringProcessor.NormalizeVariant(itemDto.Variant);

        return document.Items.FirstOrDefault(i =>
            string.Equals(i.ItemNumber, itemDto.ItemNumber, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(StringProcessor.NormalizeVariant(i.Variant), targetVariant,
                StringComparison.OrdinalIgnoreCase) &&
            string.Equals(i.BinCode, itemDto.BinCode, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(i.LotNumber, itemDto.LotNumber, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<DistributionScanStatus> ResolveScanStatusAsync(DistributionScanRequest request, DistributionItem? item,
        DcxMsItemCrossReference? crossReference)
    {
        if (item is null)
        {
            return DistributionScanStatus.NotFound;
        }

        if (!IsBarcodeMatchingItem(request, crossReference))
        {
            return DistributionScanStatus.NotFound;
        }

        var scannedQuantity = await GetScannedQuantityAsync(request.DistributionId, item.Id);
        return scannedQuantity < item.Quantity ? DistributionScanStatus.Ok : DistributionScanStatus.Excess;
    }

    private async Task<int> GetScannedQuantityAsync(int distributionId, int itemId)
    {
        var scans = await _distributionScanRepository.GetAllAsync();
        return scans.Count(s => s.DistributionId == distributionId && s.ItemId == itemId && s.Status == DistributionScanStatus.Ok);
    }

    private static bool IsBarcodeMatchingItem(DistributionScanRequest request, DcxMsItemCrossReference? crossReference)
    {
        if (crossReference is null)
        {
            return false;
        }

        var crossVariant = StringProcessor.NormalizeVariant(crossReference.VariantCode);
        var requestVariant = StringProcessor.NormalizeVariant(request.Item.Variant);

        return string.Equals(crossReference.ItemNo, request.Item.ItemNumber, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(crossVariant, requestVariant, StringComparison.OrdinalIgnoreCase);
    }

    private static DistributionScan CreateScan(DistributionScanRequest request, int distributionId,
        DistributionDocument document, DistributionItem item, DistributionScanStatus status)
    {
        return new DistributionScan
        {
            Barcode = request.Barcode,
            BinCode = request.Item.BinCode,
            LotNumber = request.Item.LotNumber,
            ScanType = ScanType.Take,
            TimeStamp = DateTime.UtcNow,
            Status = status,
            UserId = request.UserId,
            OriginId = document.OriginId,
            DistributionId = distributionId,
            DocumentId = document.Id,
            ItemId = item.Id,
            User = null!,
            Origin = null!,
            Distribution = null!,
            Document = null,
            Item = null
        };
    }

    private static DistributionScan CreateTransientScan(DistributionScanRequest request, DistributionScanStatus status,
        int? distributionId = null, int? documentId = null, int? itemId = null)
    {
        return new DistributionScan
        {
            Barcode = request.Barcode,
            BinCode = request.Item.BinCode,
            LotNumber = request.Item.LotNumber,
            ScanType = ScanType.Take,
            TimeStamp = DateTime.UtcNow,
            Status = status,
            UserId = request.UserId,
            OriginId = 0,
            DistributionId = distributionId ?? request.DistributionId,
            DocumentId = documentId ?? request.DocumentId,
            ItemId = itemId ?? request.Item.Id,
            User = null!,
            Origin = null!,
            Distribution = null!,
            Document = null,
            Item = null
        };
    }

    public async Task<bool> DeleteScanAsync(int scanId)
    {
        try
        {
            await _distributionScanRepository.DeleteAsync(scanId);
            return true;
        }
        catch (Exception e)
        {
            Log.Error("Error deleting scan with ID {ScanId}: {EMessage}", scanId, e.Message);
            return false;
        }
    }
}
