using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Infrastructure.Authorization;
using MSWMS.Infrastructure.Helpers;
using MSWMS.Models;
using MSWMS.Models.Requests;
using MSWMS.Models.Responses;
using MSWMS.Services;

namespace MSWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly OrderService _orderService;
        private readonly DCXWMSContext _dcxContext;

        public OrderController(AppDbContext context, IMapper mapper, OrderService orderService, DCXWMSContext dcxContext)
        {
            _context = context;
            _mapper = mapper;
            _orderService = orderService;
            _dcxContext = dcxContext;
        }

        // GET: api/Order
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<OrderList>> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (pageSize > 50)
            {
                return BadRequest("Maximum order per page is 50");
            }

            var query = _context.Orders.AsNoTracking();
            
            var totalItems = await query.CountAsync();
            
            var ordersDto = await query
                .OrderBy(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var ordersList = new OrderList
            {
                Orders = ordersDto,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                PageSize = pageSize,
                CurrentPage = page
            };
            return ordersList;
        }
        
        [HttpGet("filter")]
        [Authorize]
        public async Task<ActionResult<OrderList>> GetOrdersWithFilters(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? shipmentId = null,
            [FromQuery] string? transferOrderNumber = null, 
            [FromQuery] string? transferShipmentNumber = null,
            [FromQuery] string? origin = null,
            [FromQuery] string? destination = null,
            [FromQuery] Order.OrderStatus? status = null,
            [FromQuery] Order.OrderType? type = null,
            [FromQuery] Order.OrderPriority? priority = null,
            [FromQuery] DateTime? createdFrom = null,
            [FromQuery] DateTime? createdTo = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool descending = false)
        {
            if (pageSize > 50)
            {
                return BadRequest("Maximum order per page is 50");
            }

            var query = _context.Orders.AsNoTracking();

            if (!string.IsNullOrEmpty(shipmentId))
                query = query.Where(o => o.ShipmentId.Contains(shipmentId));

            if (!string.IsNullOrEmpty(transferOrderNumber))
                query = query.Where(o => o.TransferOrderNumber.Contains(transferOrderNumber));

            if (!string.IsNullOrEmpty(transferShipmentNumber))
                query = query.Where(o => o.TransferShipmentNumber.Contains(transferShipmentNumber));

            if (!string.IsNullOrEmpty(origin))
                query = query.Where(o => o.Origin.Name.Contains(origin));

            if (!string.IsNullOrEmpty(destination))
                query = query.Where(o => o.Destination.Name.Contains(destination));

            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            if (type.HasValue)
                query = query.Where(o => o.Type == type.Value);

            if (priority.HasValue)
                query = query.Where(o => o.Priority == priority.Value);

            if (createdFrom.HasValue)
                query = query.Where(o => o.CreatedDateTime >= createdFrom.Value);

            if (createdTo.HasValue)
                query = query.Where(o => o.CreatedDateTime <= createdTo.Value);

            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "shipmentid" => descending ? query.OrderByDescending(o => o.ShipmentId) : query.OrderBy(o => o.ShipmentId),
                    "transferordernumber" => descending ? query.OrderByDescending(o => o.TransferOrderNumber) : query.OrderBy(o => o.TransferOrderNumber),
                    "transfershipmentnumber" => descending ? query.OrderByDescending(o => o.TransferShipmentNumber) : query.OrderBy(o => o.TransferShipmentNumber),
                    "createdat" => descending ? query.OrderByDescending(o => o.CreatedDateTime) : query.OrderBy(o => o.CreatedDateTime),
                    "status" => descending ? query.OrderByDescending(o => o.Status) : query.OrderBy(o => o.Status),
                    "type" => descending ? query.OrderByDescending(o => o.Type) : query.OrderBy(o => o.Type),
                    "priority" => descending ? query.OrderByDescending(o => o.Priority) : query.OrderBy(o => o.Priority),
                    _ => descending ? query.OrderByDescending(o => o.Id) : query.OrderBy(o => o.Id)
                };
            }
            else
            {
                query = query.OrderBy(o => o.Id);
            }
            
            var totalItems = await query.CountAsync();
            
            var ordersDto = await query
                .AsNoTracking()
                .AsSplitQuery()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var ordersList = new OrderList
            {
                Orders = ordersDto,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                PageSize = pageSize,
                CurrentPage = page
            };
            return ordersList;
        }


        [HttpGet("details/{id}")]
        [Authorize(Policy = Policies.RequirePicker)]
        public async Task<ActionResult<Order?>> GetOrderDetails(int id)
        {
            return await _context.Orders
                .AsSplitQuery()
                .Include(o => o.Items)
                .ThenInclude(i => i.ItemInfo.Take(1))
                .Include(o => o.Destination)
                .AsNoTracking() 
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        [Authorize(Policy = Policies.RequireAdmin)]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = Policies.RequireManager)]
        public async Task<IActionResult> PutOrder(int id, CreateOrderRequest orderRequest)
        {

            return Forbid(); // not implemented yet
            var order = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.CreatedBy)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.ShipmentId != orderRequest.ShipmentId && 
                await _context.Orders.AnyAsync(o => o.ShipmentId == orderRequest.ShipmentId))
            {
                return BadRequest("Order with this shipment id already exists");
            }

            if (order.TransferShipmentNumber != orderRequest.TransferShipmentNumber && 
                await _context.Orders.AnyAsync(o => o.TransferShipmentNumber == orderRequest.TransferShipmentNumber))
            {
                return BadRequest("Order with this transfer shipment already exists");
            }

            orderRequest.UserId = order.CreatedBy.Id;

            var newOrderState = await orderRequest.ToEntity(_context, _dcxContext);

            order.ShipmentId = newOrderState.ShipmentId;
            order.TransferOrderNumber = newOrderState.TransferOrderNumber;
            order.TransferShipmentNumber = newOrderState.TransferShipmentNumber;
            order.Origin = newOrderState.Origin;
            order.Destination = newOrderState.Destination;
            order.Status = newOrderState.Status;
            order.Type = newOrderState.Type;
            order.Priority = newOrderState.Priority;
            
            if (order.Items != null && order.Items.Any())
            {
                _context.RemoveRange(order.Items);
            }
            order.Items = newOrderState.Items;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("reset/{orderId}")]
        [Authorize(Policy = Policies.RequireManager)]
        public async Task<ActionResult> ResetOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Scans)
                .Include(o => o.Boxes)
                .FirstOrDefaultAsync(o => o.Id == orderId);
    
            if (order == null)
            {
                return BadRequest("Order not found");
            }

            if (await _context.Shipments.AnyAsync(s => s.Orders.Any(o => o.Id == orderId)))
            {
                return BadRequest("Order has shipments");
            }
    
            if (order.Scans != null && order.Scans.Count != 0)
            {
                _context.RemoveRange(order.Scans);
            }
    
            if (order.Boxes != null && order.Boxes.Count != 0)
            {
                _context.RemoveRange(order.Boxes);
            }
    
            order.Status = Order.OrderStatus.New;
    
            await _context.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPost]
        [Authorize(Policy = Policies.RequireManager)]
        public async Task<ActionResult<Order>> PostOrder(CreateOrderRequest orderRequest)
        {
            if (_context.Orders.Any(o => o.ShipmentId == orderRequest.ShipmentId))
            {
                return BadRequest("Order with this shipment id already exists");
            }

            if (_context.Orders.Any(o => o.TransferShipmentNumber == orderRequest.TransferShipmentNumber))
            {
                return BadRequest("Order with this transfer shipment already exists");
            }
            
            var user = _context.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
            orderRequest.UserId = user.Id;

            var order = orderRequest.ToEntity(_context, _dcxContext).Result;
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.RequireAdmin)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("create/{ts}")]
        [Authorize(Policy = Policies.RequireAdmin)]
        public async Task<ActionResult<Order>> CreateOrder(string ts)
        {
            var order = await _orderService.CreateOrder(ts);
            return Ok(order);
        }

        [HttpGet("get-id/{no}")]
        [Authorize(Policy = Policies.RequirePicker)]
        public async Task<ActionResult<int>> GetOrderId(string no)
        {
            var dbNo = "";
            
            if (no.StartsWith("db-", StringComparison.CurrentCultureIgnoreCase))
            {
                dbNo = no;
                no = await _orderService.GetShipmentNumberByShippingId(no);
            }
            var id = await _orderService.LocallyExists(no);

            if (id is not null) return id;
            
            var order = await _orderService.CreateOrder(no, dbNo);
                
            if (order is null) return NotFound();
                
            id = order.Id;

            return id;
        }

        [HttpPost("update-remark/{id}")]
        [Authorize(Policy = Policies.RequireManagerOrDispatcher)]
        public async Task<ActionResult> UpdateOrderRemark(int id, string remark)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            
            order.Remark = remark;
            await _context.SaveChangesAsync();
            
            return Ok();
        }
        
        // GET: api/Order/shipment-id-exist
        [HttpGet("shipment-id-exists")]
        public async Task<ActionResult<bool>> IsShipmentIdExists(string shipmentId)
        {
            return await ShipmentIdExist(shipmentId);
        }
        
        // GET: api/Order/shipment-id-exist
        [HttpGet("order-number-exists")]
        public async Task<ActionResult<bool>> IsTransferOrderExists(string transferOrderNumber)
        {
            return await TransferOrderExists(transferOrderNumber);
        }

        [HttpGet("shipment-number-exists")]
        public async Task<ActionResult<bool>> IsTransferShipmentNumberExists(string transferShipmentNumber)
        {
            return await _context.Orders.AnyAsync(o => o.TransferShipmentNumber == transferShipmentNumber);
        }


        private Task<bool> ShipmentIdExist(string shipmentId)
        {
            return _context.Orders.AnyAsync(o => o.ShipmentId == shipmentId);
        }

        private Task<bool> TransferOrderExists(string transferOrderNumber)
        {
            return _context.Orders.AnyAsync(o => o.TransferOrderNumber == transferOrderNumber);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        // POST: api/Order/upload-excel
        [HttpPost("upload-excel")]
        [Authorize(Policy = Policies.RequireManager)]
        public async Task<ActionResult<ExcelParsedOrder>> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var parser = new OrderExcelParser();
            var extension = Path.GetExtension(file.FileName);
            var tempFilePath = Path.GetTempFileName() + extension;

            await using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var parsedOrder = await parser.Parse(tempFilePath);

            return parsedOrder;
        }
    }
}
