using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    private readonly IHubContext<ScanHub> _hubContext;

    public ScanController(AppDbContext context, IScanService scanService, IHubContext<ScanHub> hubContext)
    {
        _context = context;
        _scanService = scanService;
        _hubContext = hubContext;
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<ActionResult<ScanResponse?>> GetScan(int id)
    {
        var scan = await _context.Scans
            .AsNoTracking()
            .Include(s => s.Box)
            .Include(s => s.User)
            .Include(s => s.Item)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        return new ScanResponse
        {
            Id = scan.Id,
            ItemId = scan.Item?.Id??0,
            Barcode = scan.Barcode,
            TimeStamp = scan.TimeStamp,
            Status = scan.Status,
            BoxNumber = scan.Box.BoxNumber,
            BoxId = scan.Box.Id,
            UserId = scan.User.Id,
            Username = scan.User.Username,
        };
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
        Console.WriteLine(scanRequest.BoxNumber);
        Console.WriteLine(scanRequest.OrderId);
        Console.WriteLine(scanRequest.UserId);
        Console.WriteLine(scanRequest.Barcode);
        
        var startTime = DateTime.Now;
        var userName = User.Identity?.Name;
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == userName);
        
        if (user is null) return BadRequest();
        
        scanRequest.UserId = user.Id;
        
        var response = await _scanService.ProcessScan(scanRequest);
        
        Console.WriteLine($"Time before SignalR: {DateTime.Now - startTime}");

        var groupName = $"Order_{response.OrderId}";
        await _hubContext.Clients.Group(groupName).SendAsync("scanProcessed", response.OrderId, response.Id, response.BoxNumber, response.BoxId, response.ItemId);

        Console.WriteLine($"Time for scan: {DateTime.Now - startTime}");
        
        Console.WriteLine("Send To Group" + groupName);
        Console.WriteLine("respone box id " + response.BoxId);
        return response;
    }
}