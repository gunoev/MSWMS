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
        string? shippingId = null,
        string? transferShipmentNumber = null,
        string? transferOrderNumber = null)
    {
        transferShipmentNumber = transferShipmentNumber?.Trim();
        transferOrderNumber = transferOrderNumber?.Trim();
        shippingId = shippingId?.Trim();

        var hasTransferSearch = !string.IsNullOrWhiteSpace(transferShipmentNumber) ||
                                !string.IsNullOrWhiteSpace(transferOrderNumber) || 
                                !string.IsNullOrWhiteSpace(shippingId);

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

            if (!string.IsNullOrWhiteSpace(shippingId))
            {
                var shippingIdSearch = shippingId.ToLower();
                query = query.Where(s => s.Order.ShipmentId.ToLower() == shippingIdSearch);
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

        var scans = await query
            .OrderBy(s => s.TimeStamp)
            .Select(s => new
            {
                s.TimeStamp,
                s.Barcode,
                OrderId = s.Order.Id,
                OrderNumber = s.Order.TransferOrderNumber,
                ShipmentNumber = s.Order.TransferShipmentNumber,
                UserId = s.User.Id,
                Username = s.User.Username,
                Origin = s.Order.Origin.Name,
                Destination = s.Order.Destination.Name,
                OriginCode = s.Order.Origin.Code,
                DestinationCode = s.Order.Destination.Code,
                BoxId = s.Box.UniqueId ?? s.Box.Guid.ToString(),
                s.Box.BoxNumber
            })
            .ToListAsync();

        if (scans.Count == 0)
        {
            return [];
        }

        var orderIds = scans
            .Select(s => s.OrderId)
            .Distinct()
            .ToList();

        var barcodes = scans
            .Select(s => s.Barcode)
            .Where(b => !string.IsNullOrWhiteSpace(b))
            .Distinct()
            .ToList();

        var orderStats = await _context.Orders
            .AsNoTracking()
            .Where(o => orderIds.Contains(o.Id))
            .Select(o => new
            {
                o.Id,
                TotalQuantity = o.Items.Sum(i => (int?)i.NeededQuantity) ?? 0,
                TotalScanned = o.Scans.Count(s => s.Status == Scan.ScanStatus.Ok),
                TotalBoxes = o.Boxes.Count()
            })
            .ToDictionaryAsync(o => o.Id);

        var itemInfos = await _context.ItemInfos
            .AsNoTracking()
            .Where(ii => barcodes.Contains(ii.Barcode))
            .OrderBy(ii => ii.Id)
            .Select(ii => new
            {
                ii.Barcode,
                ii.ItemNumber,
                ii.Variant,
                ii.Description
            })
            .ToListAsync();

        var itemInfoByBarcode = itemInfos
            .GroupBy(ii => ii.Barcode)
            .ToDictionary(g => g.Key, g => g.First());

        return scans.Select(scan =>
        {
            var orderStat = orderStats.GetValueOrDefault(scan.OrderId);
            itemInfoByBarcode.TryGetValue(scan.Barcode, out var itemInfo);

            return new PricingScanRow
            {
                TimeStamp = scan.TimeStamp,
                Barcode = scan.Barcode,
                ItemNumber = itemInfo?.ItemNumber ?? string.Empty,
                Variant = itemInfo?.Variant ?? string.Empty,
                Description = itemInfo?.Description ?? string.Empty,
                OrderNumber = scan.OrderNumber,
                ShipmentNumber = scan.ShipmentNumber,
                OrderTotalQuantity = (orderStat?.TotalQuantity ?? 0).ToString(),
                OrderTotalScanned = (orderStat?.TotalScanned ?? 0).ToString(),
                OrderTotalBoxes = (orderStat?.TotalBoxes ?? 0).ToString(),
                UserId = scan.UserId,
                Username = scan.Username,
                Origin = scan.Origin,
                Destination = scan.Destination,
                OriginCode = scan.OriginCode,
                DestinationCode = scan.DestinationCode,
                BoxId = scan.BoxId,
                Quantity = 1,
                BoxNumber = scan.BoxNumber
            };
        }).ToList();
    }
}
