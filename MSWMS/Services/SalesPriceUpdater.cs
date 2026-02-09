using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Entities.External;
using MSWMS.Services.Interfaces;

namespace MSWMS.Services;

public sealed class SalesPriceUpdater : ISalesPriceUpdater
{
    private readonly ExternalReadOnlyContext _externalContext;
    private readonly AppDbContext _context;

    public SalesPriceUpdater(ExternalReadOnlyContext externalContext, AppDbContext context)
    {
        _externalContext = externalContext;
        _context = context;
    }

    public async Task<int> UpdateItemInfosPricesAsync(DateTime fromDate, CancellationToken ct = default)
    {
        if (fromDate < DateTime.Now.AddDays(-7))
            throw new ArgumentOutOfRangeException(nameof(fromDate), "Cannot update prices older than 7 days");

        var itemsNumbers = _externalContext.MikesportCoSALSalesPrices
            .AsNoTracking()
            .Where(sp => sp.StartingDate >= fromDate.Date)
            .Select(sp => sp.ItemNo)
            .Distinct();

        var updatedItems = await _externalContext.MikesportCoSALDefaultSalesPrices
            .AsNoTracking()
            .Where(sp => itemsNumbers.Contains(sp.ItemNo))
            .ToListAsync(ct);

        if (updatedItems.Count == 0)
            return 0;

        foreach (var item in updatedItems)
        {
            var itemInfos = await _context.ItemInfos
                .Where(inf => inf.ItemNumber == item.ItemNo)
                .ToListAsync(ct);

            if (itemInfos.Count == 0) continue;

            foreach (var itemInfo in itemInfos)
            {
                itemInfo.Price = item.UnitPriceIncludingVatCurre;
                itemInfo.DiscountPrice = item.DiscountedPriceCurrency;
                itemInfo.Currency = item.Currency;
                itemInfo.DiscountPercentage = item.DiscountPercentage;
            }
        }

        await _context.SaveChangesAsync(ct);
        return updatedItems.Count;
    }
}