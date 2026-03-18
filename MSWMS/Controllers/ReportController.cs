using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSWMS.Infrastructure.Authorization;
using MSWMS.Models.DTO.Responses.Reports;
using MSWMS.Services;

namespace MSWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("pricing-scans")]
    public async Task<ActionResult<List<PricingScanRow>>> GetPricingScansReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? shippingId = null,
        [FromQuery] string? transferShipmentNumber = null,
        [FromQuery] string? transferOrderNumber = null)
    {
        var hasTransferSearch = !string.IsNullOrWhiteSpace(transferShipmentNumber) ||
                                !string.IsNullOrWhiteSpace(transferOrderNumber) ||
                                !string.IsNullOrWhiteSpace(shippingId);

        if (!hasTransferSearch && (!startDate.HasValue || !endDate.HasValue))
        {
            return BadRequest("startDate and endDate are required when transfer search params are not provided");
        }

        if (!hasTransferSearch && startDate > endDate)
        {
            return BadRequest("startDate must be less than or equal to endDate");
        }

        if (!hasTransferSearch && (endDate!.Value.Date - startDate!.Value.Date).TotalDays > 7)
        {
            return BadRequest("Date range cannot exceed 7 days");
        }

        var rows = await _reportService.GetPricingScanReportAsync(
            startDate,
            endDate,
            shippingId,
            transferShipmentNumber,
            transferOrderNumber);
        return Ok(rows);
    }
}
