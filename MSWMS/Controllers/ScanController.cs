using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Hubs;
using MSWMS.Infrastructure.Authorization;
using MSWMS.Models.DTO.Responses;
using MSWMS.Models.Requests;
using MSWMS.Models.Responses;
using MSWMS.Services;
using MSWMS.Services.Interfaces;

namespace MSWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScanController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IScanService _scanService;
    private readonly OrderService _orderService;
    private readonly IHubContext<ScanHub> _hubContext;
    private readonly IMapper _mapper;

    public ScanController(
        AppDbContext context, 
        IScanService scanService, 
        IHubContext<ScanHub> hubContext, 
        IMapper mapper, 
        OrderService orderService)
    {
        _context = context;
        _scanService = scanService;
        _hubContext = hubContext;
        _mapper = mapper;
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<ActionResult<ScanDto?>> GetScan(int id)
    {
        var scan = await _context.Scans
            .AsNoTracking()
            .Include(s => s.Box)
            .Include(s => s.User)
            .Include(s => s.Item)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        return new ScanDto
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
    public async Task<ActionResult<IEnumerable<ScanDto>>> GetScansByOrderId(int id)
    {
        var scans = await _context.Scans
            .AsNoTracking()
            .Include(s => s.Box)
            .Include(s => s.User)
            .Where(s => s.Order.Id == id)
            .ProjectTo<ScanDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return scans;
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

    [HttpDelete("delete-many/{orderId}")]
    [Authorize(Policy = Policies.RequirePicker)]
    public async Task<IActionResult> DeleteScans(IEnumerable<int> ids, int orderId)
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
            .ThenInclude(b => b.Scans)
            .Include(s => s.Order)
            .Include(s => s.Item)
            .ThenInclude(i => i.ItemInfo)
            .Where(s => ids.Contains(s.Id) && s.User.Username == userName && s.Order.Id == orderId)
            .ToListAsync();

        if (!scans.Any())
        {
            return NotFound();
        }
        
        var boxIds = scans.Select(s => s.Box.Id).Distinct().ToList();

        _context.Scans.RemoveRange(scans);
        await _context.SaveChangesAsync();
        
        ItemDto CalcItemDto(Scan scan)
        {
            var dto = _mapper.Map<ItemDto>(scan.Item);
            dto.Scanned = (uint)_scanService.GetScannedQuantity(scan.Item, scan.Order).Result;
            dto.Remaining = (int)(scan.Item.NeededQuantity - dto.Scanned);
            return dto;
        }

        var scanInfos = scans.Select(s => new ScanResponse
        {
            Scan = _mapper.Map<ScanDto>(s),
            Box = _mapper.Map<BoxDto>(s.Box),
            Item = CalcItemDto(s),
        }).ToList();

        foreach (var info in scanInfos)
        {
            var groupName = $"Order_{orderId}";
            await _hubContext.Clients.Group(groupName)
                .SendAsync("scanDeleted", info);
        }

        var boxesToCheck = await _context.Boxes
            .Include(b => b.Scans)
            .Where(b => boxIds.Contains(b.Id))
            .ToListAsync();

        var emptyBoxes = boxesToCheck.Where(b => b.Scans == null || !b.Scans.Any()).ToList();

        if (emptyBoxes.Any())
        {
            foreach (var box in emptyBoxes)
            {
                _context.Boxes.Remove(box);

                await _hubContext.Clients.Group($"Order_{orderId}")
                    .SendAsync("boxDeleted", orderId, box.Id, box.BoxNumber);
            }
            
            await _context.SaveChangesAsync();
        }

        var order = await _context.Orders.FindAsync(orderId);
        
        if (order is not null)
        {
            await _orderService.UpdateOrderStatus(order.Id);   
        }

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
        
        if (response is null) return BadRequest("Order not found");
        
        Console.WriteLine($"Time before SignalR: {DateTime.Now - startTime}");

        var groupName = $"Order_{response.Scan.OrderId}";
        await _hubContext.Clients.Group(groupName).SendAsync("scanProcessed", response);

        Console.WriteLine($"Time for scan: {DateTime.Now - startTime}");
        
        Console.WriteLine("Send To Group" + groupName);
        Console.WriteLine("respone box id " + response.Scan.BoxId);
        
        return Ok(response);
    }
}