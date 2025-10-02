using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Infrastructure.Authorization;
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

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<ActionResult<Scan?>> GetScan(int id)
    {
        return await _context.Scans.FindAsync(id);
    }

    
    [HttpPost]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<ActionResult<ScanResponse>> PostScan(ScanRequest scanRequest)
    {
        var status = await _scanService.ProcessScan(scanRequest);
        
        var response = new ScanResponse
        {
            Barcode = scanRequest.Barcode, 
            TimeStamp = DateTime.Now,
            Status = status,
            BoxNumber = scanRequest.BoxNumber,
            Username = _context.Users.AsNoTracking().First(u => u.Id == scanRequest.UserId).Username,
        };

        return response;
    }
}