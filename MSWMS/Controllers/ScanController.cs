using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Hubs;
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
    private readonly ScanHub _scanHub;

    public ScanController(AppDbContext context, IScanService scanService, ScanHub scanHub)
    {
        _context = context;
        _scanService = scanService;
        _scanHub = scanHub;
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<ActionResult<Scan?>> GetScan(int id)
    {
        return await _context.Scans.FindAsync(id);
    }

    [HttpGet("order/{id}")]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<ActionResult<IEnumerable<ScanResponse>>> GetScansByOrderId(int id)
    {
        var scans = await _context.Scans.AsNoTracking().Include(s => s.Box).Include(s => s.User).Where(s => s.Order.Id == id).ToListAsync();
        
        var scansDto = new List<ScanResponse>();

        foreach (var scan in scans)
        {
            var scanDto = new ScanResponse
            {
                Id = scan.Id,
                Barcode = scan.Barcode,
                TimeStamp = scan.TimeStamp,
                Status = scan.Status,
                BoxNumber = scan.Box.BoxNumber,
                UserId = scan.User.Id,
                Username = scan.User.Username,
            };
            
            scansDto.Add(scanDto);
        }

        return scansDto;
    }

    
    [HttpPost]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<ActionResult<ScanResponse>> PostScan(ScanRequest scanRequest)
    {
        var userName = User.Identity?.Name;
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == userName);
        
        if (user is null) return BadRequest();
        
        scanRequest.UserId = user.Id;
        
        var response = await _scanService.ProcessScan(scanRequest);

        await _scanHub.ScanProcessed(scanRequest.OrderId, response.Id, response.BoxNumber);

        return response;
    }
}