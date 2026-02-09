namespace MSWMS.Services.Interfaces;

public interface ISalesPriceUpdater
{
    Task<int> UpdateItemInfosPricesAsync(DateTime fromDate, CancellationToken ct = default);
}