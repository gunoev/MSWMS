using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSWMS.Entities;
using MSWMS.Entities.External;
using MSWMS.Infrastructure.Authorization;
using MSWMS.Services.Interfaces;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesPriceController : ControllerBase
    {
        private readonly ExternalReadOnlyContext _externalContext;
        private AppDbContext _context;
        private readonly ISalesPriceUpdater _updater;
        
        public SalesPriceController(ExternalReadOnlyContext externalContext, AppDbContext context, ISalesPriceUpdater updater)
        {
            _externalContext = externalContext;
            _context = context;
            _updater = updater;
        }
        
        [HttpGet]
        public IActionResult GetSalesPrices()
        {
            var salesPrices = _externalContext.MikesportCoSALSalesPrices
                .Where(sp => sp.StartingDate == DateTime.Now.Date)
                .Take(10)
                .ToList();
            
            return Ok(salesPrices);
        }
        
        [HttpGet("default")]
        public IActionResult GetDefaultSalesPrices()
        {
            var defaultSalesPrices = _externalContext.MikesportCoSALDefaultSalesPrices.Take(10).ToList();
            return Ok(defaultSalesPrices);
        }

        [Authorize(Policy = Policies.RequireAdmin)]
        [HttpPost("update-prices")]
        public async Task<IActionResult> UpdateItemInfosPrices([FromQuery] DateTime fromDate, CancellationToken ct)
        {
            if (fromDate < DateTime.Now.AddDays(-7))
                return BadRequest("Cannot update prices older than 7 days");

            var updatedCount = await _updater.UpdateItemInfosPricesAsync(fromDate, ct);

            if (updatedCount == 0)
                return NotFound("No items found for update");

            return Ok(new { updatedCount, fromDate = fromDate.Date });
        }
    }
}
