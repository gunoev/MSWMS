using Microsoft.AspNetCore.Mvc;
using MSWMS.Entities;
using MSWMS.Models.Requests;
using MSWMS.Models.Responses;

namespace MSWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScanController : ControllerBase
{
    private readonly AppDbContext _context;

    public ScanController(AppDbContext context)
    {
        _context = context;
    }

    // Auth
    [HttpPost]
    public async Task<ActionResult<ScanResponse>> PostScan(ScanRequest scanRequest)
    {
        // scan

        return new ScanResponse{Barcode = "123", TimeStamp = DateTime.Now};
    }
}