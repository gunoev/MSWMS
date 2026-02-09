using Quartz;
using MSWMS.Services.Interfaces;

namespace MSWMS.Jobs;

[DisallowConcurrentExecution] // чтобы не было параллельных запусков
public sealed class UpdatePricesJob : IJob
{
    private readonly ISalesPriceUpdater _updater;
    private readonly ILogger<UpdatePricesJob> _logger;

    public UpdatePricesJob(ISalesPriceUpdater updater, ILogger<UpdatePricesJob> logger)
    {
        _updater = updater;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var fromDate = DateTime.Today;
        var updated = await _updater.UpdateItemInfosPricesAsync(fromDate, context.CancellationToken);

        _logger.LogInformation("UpdatePricesJob finished. Updated items: {Count}. FromDate: {FromDate}", updated, fromDate);
    }
}