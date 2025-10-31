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

    [HttpGet("count/{fromDate}/to/{toDate}")]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<ActionResult<object>> CountByDateRange(string fromDate, string toDate)
    {
        var toDateTime = DateTime.Parse(toDate);
        var fromDateTime = DateTime.Parse(fromDate);
        
        var user = User.Identity?.Name;
        
        var count = await _context.Scans
            .AsNoTracking()
            .Where(s => s.TimeStamp >= fromDateTime && s.TimeStamp <= toDateTime && s.User.Username == user)
            .CountAsync();

        return count;
    }
    
    [HttpGet("statistic/{fromDate}/to/{toDate}")]
    [Authorize(Policy = Policies.RequireManager)]
    public async Task<ActionResult<object>> StatisticByDateRange(string fromDate, string toDate)
    {
        var toDateTime = DateTime.Parse(toDate);
        var fromDateTime = DateTime.Parse(fromDate);

        var days = (toDateTime - fromDateTime).Days + 1;
        var dailyStats = new List<object>();

        for (var i = 0; i < days; i++)
        {
            var currentDate = fromDateTime.AddDays(i);
            var nextDate = currentDate.AddDays(1);

            var stats = new
            {
                Date = currentDate.Date,
                TotalScans = await _context.Scans
                    .AsNoTracking()
                    .Where(s => s.TimeStamp >= currentDate && s.TimeStamp < nextDate)
                    .CountAsync(),

                TotalBoxes = await _context.Scans
                    .AsNoTracking()
                    .Where(s => s.TimeStamp >= currentDate && s.TimeStamp < nextDate)
                    .Select(s => s.Box.Id)
                    .Distinct()
                    .CountAsync(),

                Users = new List<object>()
            };

            var users = await _context.Users
                .AsNoTracking()
                .Where
                    (u => _context.Scans
                        .Any(s => s.TimeStamp >= currentDate 
                                  && s.TimeStamp < nextDate && s.User.Id == u.Id))
                .ToListAsync();

            foreach (var user in users)
            {
                var scans = await _context.Scans
                    .AsNoTracking()
                    .Where(s => s.TimeStamp >= currentDate && 
                               s.TimeStamp < nextDate && 
                               s.User.Username == user.Username)
                    .CountAsync();

                var boxes = await _context.Scans
                    .AsNoTracking()
                    .Where(s => s.TimeStamp >= currentDate && 
                               s.TimeStamp < nextDate && 
                               s.User.Username == user.Username)
                    .Select(s => s.Box.Id)
                    .Distinct()
                    .CountAsync();

                stats.Users.Add(new
                {
                    Username = user.Username,
                    Name = user.Name,
                    TotalScans = scans,
                    TotalBoxes = boxes
                });
            }

            dailyStats.Add(stats);
        }

        return dailyStats;
    }

    [HttpGet("order/{id}")]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<ActionResult<IEnumerable<ScanResponse>>> GetScansByOrderId(int id)
    {
        var scans = await _context.Scans
            .AsNoTracking()
            .Include(s => s.Box)
            .Include(s => s.User)
            .Where(s => s.Order.Id == id)
            .ToListAsync();
        
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

    [HttpDelete]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<IActionResult> DeleteScan(int id)
    {
        var scan = await _context.Scans.FindAsync(id);
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
        
        if (user == null)
        {
            return BadRequest();
        }
        
        if (scan == null)
        {
            return NotFound();
        }

        if (scan.User != user || !user.Roles.Any(r => r.Type is Role.RoleType.Manager or Role.RoleType.Admin))
        {
            return BadRequest("You are not allowed to delete this scan");
        }

        _context.Scans.Remove(scan);

        if (scan.Box.Scans.Count == 0)
        {
            _context.Boxes.Remove(scan.Box);
        }
        
        await _context.SaveChangesAsync();
        
        var groupName = $"Order_{scan.Order.Id}";
        await _hubContext.Clients.Group(groupName)
            .SendAsync("scanDeleted", scan.Order.Id, scan.Id, scan.Box.Id, scan.Box.BoxNumber, scan.Item.Id);


        return NoContent();

    }

    [HttpDelete("delete-many")]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<IActionResult> DeleteScans(IEnumerable<int> ids)
    {
        var userName = User.Identity?.Name;
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == userName);

        if (user == null)
        {
            return BadRequest();
        }

        var scans = await _context.Scans
            .Include(s => s.Box)
            .Include(s => s.Order)
            .Include(s => s.Item)
            .Where(s => ids.Contains(s.Id) && s.User.Username == userName)
            .ToListAsync();

        if (!scans.Any())
        {
            return NotFound();
        }

        foreach (var scan in scans)
        {
            _context.Scans.Remove(scan);
            
            await _context.SaveChangesAsync();

            var box = await _context.Boxes
                .Include(b => b.Scans)
                .FirstOrDefaultAsync(b => b.Id == scan.Box.Id);

            if (box != null && box.Scans?.Count == 0)
            {
                _context.Boxes.Remove(scan.Box);
                
                await _hubContext.Clients.Group($"Order_{scan.Order.Id}").SendAsync("boxDeleted", scan.Order.Id, box.Id, box.BoxNumber);
            }

            var groupName = $"Order_{scan.Order.Id}";
            await _hubContext.Clients.Group(groupName)
                .SendAsync("scanDeleted", scan.Order.Id, scan.Id, scan.Box.Id, scan.Box.BoxNumber, scan.Item?.Id);
        }

        await _context.SaveChangesAsync();
        return NoContent();
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