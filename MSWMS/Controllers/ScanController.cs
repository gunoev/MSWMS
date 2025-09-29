using Microsoft.AspNetCore.Mvc;
using MSWMS.Entities;
using MSWMS.Models.Requests;
using MSWMS.Models.Responses;
using MSWMS.Services.Interfaces;

namespace MSWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScanController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IScanService _scanService;

    public ScanController(AppDbContext context, IScanService scanService)
    {
        _context = context;
        _scanService = scanService;
    }

    // Auth
    [HttpPost]
    public async Task<ActionResult<ScanResponse>> PostScan(ScanRequest scanRequest)
    {
        var status = await _scanService.ProcessScan(scanRequest);
        
        var response = new ScanResponse
        {
            Barcode = scanRequest.Barcode, 
            TimeStamp = DateTime.Now,
            Status = status,
            BoxNumber = scanRequest.BoxNumber,
        };

        return response;
    }
}