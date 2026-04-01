using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Models.DTO.Responses.Reports;

namespace MSWMS.Services;

public class ReportService
{
    private readonly AppDbContext _context;
    
    public ReportService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<PricingScanRow>> GetPricingScanReportAsync(
        DateTime? startDate,
        DateTime? endDate,
        string? transferShipmentNumber = null,
        string? transferOrderNumber = null)
    {
        transferShipmentNumber = transferShipmentNumber?.Trim();
        transferOrderNumber = transferOrderNumber?.Trim();

        var hasTransferSearch = !string.IsNullOrWhiteSpace(transferShipmentNumber) ||
                                !string.IsNullOrWhiteSpace(transferOrderNumber);

        var query = _context.Scans
            .AsNoTracking()
            .Where(s => s.Status == Scan.ScanStatus.Ok);

        if (hasTransferSearch)
        {
            if (!string.IsNullOrWhiteSpace(transferShipmentNumber))
            {
                var shipmentSearch = transferShipmentNumber.ToLower();
                query = query.Where(s => s.Order.TransferShipmentNumber.ToLower() == shipmentSearch);
            }

            if (!string.IsNullOrWhiteSpace(transferOrderNumber))
            {
                var orderSearch = transferOrderNumber.ToLower();
                query = query.Where(s => s.Order.TransferOrderNumber.ToLower() == orderSearch);
            }
        }
        else if (startDate.HasValue && endDate.HasValue)
        {
            if ((endDate.Value.Date - startDate.Value.Date).TotalDays > 7)
            {
                throw new ArgumentException("Date range cannot exceed 7 days");
            }

            var startDateInclusive = startDate.Value.Date;
            var endDateExclusive = endDate.Value.Date.AddDays(1);

            query = query.Where(s => s.TimeStamp >= startDateInclusive && s.TimeStamp < endDateExclusive);
        }

        var pricingScanRows = await query
            .OrderBy(s => s.TimeStamp)
            .Select(s => new PricingScanRow
            {
                TimeStamp = s.TimeStamp,
                Barcode = s.Barcode,
                ItemNumber = s.Item != null
                    ? s.Item.ItemInfo.Select(ii => ii.ItemNumber).FirstOrDefault() ?? string.Empty
                    : _context.ItemInfos.Where(ii => ii.Barcode == s.Barcode).Select(ii => ii.ItemNumber).FirstOrDefault() ?? string.Empty,
                Variant = s.Item != null
                    ? s.Item.ItemInfo.Select(ii => ii.Variant).FirstOrDefault() ?? string.Empty
                    : _context.ItemInfos.Where(ii => ii.Barcode == s.Barcode).Select(ii => ii.Variant).FirstOrDefault() ?? string.Empty,
                Description = s.Item != null
                    ? s.Item.ItemInfo.Select(ii => ii.Description).FirstOrDefault() ?? string.Empty
                    : _context.ItemInfos.Where(ii => ii.Barcode == s.Barcode).Select(ii => ii.Description).FirstOrDefault() ?? string.Empty,
                OrderNumber = s.Order.TransferOrderNumber,
                ShipmentNumber = s.Order.TransferShipmentNumber,
                OrderTotalQuantity = s.Order.Items.Sum(i => (int?)i.NeededQuantity).GetValueOrDefault().ToString(),
                OrderTotalScanned = (s.Order.Scans != null
                    ? s.Order.Scans.Count(os => os.Status == Scan.ScanStatus.Ok)
                    : 0).ToString(),
                OrderTotalBoxes = (s.Order.Boxes != null ? s.Order.Boxes.Count : 0).ToString(),
                UserId = s.User.Id,
                Username = s.User.Username,
                Origin = s.Order.Origin.Name,
                Destination = s.Order.Destination.Name,
                OriginCode = s.Order.Origin.Code,
                DestinationCode = s.Order.Destination.Code,
                BoxId = s.Box.UniqueId ?? s.Box.Guid.ToString(),
                Quantity = 1,
                BoxNumber = s.Box.BoxNumber
            })
            .ToListAsync();
        
        return pricingScanRows;
    }
}
