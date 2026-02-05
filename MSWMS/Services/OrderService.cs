using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Entities.External;
using MSWMS.Infrastructure.Helpers;
using MSWMS.Models;
using MSWMS.Models.Requests;
using MSWMS.Repositories;

namespace MSWMS.Services;

public class OrderService
{
    private IOrderRepository _orderRepository;
    private readonly AppDbContext _context;
    private readonly ExternalReadOnlyContext _externalContext;
    private readonly DCXWMSContext _dcxContext;
    
    public OrderService(IOrderRepository orderRepository, AppDbContext context, ExternalReadOnlyContext externalContext, DCXWMSContext dcxContext)
    {
        _orderRepository = orderRepository;
        _context = context;
        _externalContext = externalContext;
        _dcxContext = dcxContext;
    }
    
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<Order?> CreateOrder(string shipmentNumber, string shippingId = "")
    {
        Order? order = null;
            
        if (shipmentNumber.Contains("SS"))
        {
            order = await CreateSalesOrder(shipmentNumber);
        }
        else if (shipmentNumber.Contains("TS"))
        {
            order = await CreateTransferOrder(shipmentNumber, shippingId);
        }

        return order;
    }

    private async Task<Order?> CreateTransferOrder(string shipmentNumber, string shippingId = "")
    {
        var shipment =
            await _externalContext.MikesportCoSALTransferShipmentHeader
                .Include(tsh => tsh.Lines)
                .Select(tsh => new 
                {
                    No = tsh.No,
                    TransferFromCode = tsh.TransferFromCode,
                    TransferFromName = tsh.TransferFromName,
                    TransferToCode = tsh.TransferToCode,
                    TransferToName = tsh.TransferToName,
                    TransferOrderDate = tsh.TransferOrderDate,
                    PostingDate = tsh.PostingDate,
                    TransferOrderNo = tsh.TransferOrderNo,
                    TransferFromContact = tsh.TransferFromContact,
                    TransferToContact = tsh.TransferToContact,
                    ExternalDocumentNo = tsh.ExternalDocumentNo,
                    Lines = tsh.Lines
                        .Where(l => l.Quantity > 0)
                        .Select(l => new 
                    {
                        ItemNo = l.ItemNo,
                        VariantCode = l.VariantCode,
                        Quantity = l.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync(tsh => tsh.No == shipmentNumber);

        if (shipment == null) return null;
        
        var createOrderRequest = new CreateOrderRequest
        {
            ShipmentId = string.IsNullOrEmpty(shippingId) ? shipment.No : shippingId,
            TransferOrderNumber = shipment.TransferOrderNo,
            TransferShipmentNumber = shipment.No,
            OriginId = _context.Locations.FirstOrDefault(l => l.Code == shipment.TransferFromCode)?.Id ?? 1,
            DestinationId = _context.Locations.FirstOrDefault(l => l.Code == shipment.TransferToCode)?.Id ?? 0,
            UserId = 1,
            Priority = Order.OrderPriority.Medium,
            Type = Order.OrderType.TransferOrder,
            Items = shipment.Lines
                .Where(l => l.Quantity > 0)
                .GroupBy(l => new { l.ItemNo, l.VariantCode })
                .Select(g => new CreateOrderItemRequest
            {
                ItemNumber = g.Key.ItemNo,
                Variant = g.Key.VariantCode,
                NeededQuantity = (int)g.Sum(l => l.Quantity)
            }).ToList()
        };
        
        var order = createOrderRequest.ToEntity(_context, _dcxContext).Result;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        return order;
    }

    private async Task<Order?> CreateSalesOrder(string shipmentNumber)
    {
        var shipment =
            await _externalContext.MikesportCoSALSalesShipmentHeader
                .Include(ss => ss.Lines)
                .Select(ss => new 
                {
                    No = ss.No,
                    OrderNo = ss.OrderNo,
                    SellToCustomerNo = ss.SellToCustomerNo,
                    BillToCustomerNo = ss.BillToCustomerNo,
                    BillToName = ss.BillToName,
                    BillToAddress = ss.BillToAddress,
                    ShipToName = ss.ShipToName,
                    ShipToAddress = ss.ShipToAddress,
                    OrderDate = ss.OrderDate,
                    PostingDate = ss.PostingDate,
                    ShipmentDate = ss.ShipmentDate,
                    PostingDescription = ss.PostingDescription,
                    LocationCode = ss.LocationCode,
                    UserId = ss.UserId,
                    Lines = ss.Lines
                        .Where(l => 
                            l.GenProdPostingGroup != "SERVICES" 
                            && l.Quantity > 0
                        )
                        .Select(l => new 
                        {
                            No = l.No,
                            VariantCode = l.VariantCode,
                            Quantity = l.Quantity,
                        }).ToList()
                })
                .FirstOrDefaultAsync(ss => ss.No == shipmentNumber);
        
        if (shipment == null) return null;
        
        
        var createOrderRequest = new CreateOrderRequest
        {
            ShipmentId = shipment.No,
            TransferOrderNumber = shipment.OrderNo,
            TransferShipmentNumber = shipment.No,
            OriginId = _context.Locations.FirstOrDefault(l => l.Code == shipment.LocationCode)?.Id ?? 1,
            DestinationId = _context.Locations.FirstOrDefault(l => l.Code == "SS1LOC")?.Id ?? 0,
            UserId = 1,
            Priority = Order.OrderPriority.Medium,
            Type = Order.OrderType.SalesOrder,
            Items = shipment.Lines
                .GroupBy(l => new { l.No, l.VariantCode })
                .Select(g => new CreateOrderItemRequest
                {
                    ItemNumber = g.Key.No,
                    Variant = g.Key.VariantCode,
                    NeededQuantity = (int)g.Sum(l => l.Quantity)
                }).ToList()
        };
        
        var order = createOrderRequest.ToEntity(_context, _dcxContext).Result;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        return order;
    }

    public async Task<int?> LocallyExists(string shipmentNumber)
    {
        
        return await _context.Orders
            .Where(o => o.TransferShipmentNumber == shipmentNumber)
            .Select(o => (int?)o.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<string?> GetShipmentNumberByShippingId(string shippingId)
    {
        try
        {
            return await OrderExcelParser.GetShipmentNumber(Path.Combine("\\\\navsrv\\01-Posted Docs\\TS LIST", shippingId) + ".xlsx");
        }
        catch
        {
            return null;
        }
    }
    
    public async Task UpdateOrderStatus(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        
        if (order == null) return;
        
        var currentStatus = order.Status;
    
        var totalNeededQuantity = order.Items.Sum(i => i.NeededQuantity);
        var totalScannedQuantity = await _context.Scans.CountAsync(s => s.Order.Id == order.Id && (s.Status == Scan.ScanStatus.Ok || s.Status == Scan.ScanStatus.Excess));
    
        if (totalScannedQuantity == 0 && currentStatus != Order.OrderStatus.New)
        {
            order.Status = Order.OrderStatus.New;
        }
        else if (totalScannedQuantity < totalNeededQuantity && currentStatus != Order.OrderStatus.InProgress)
        {
            order.Status = Order.OrderStatus.InProgress; 
        }
        else if (totalScannedQuantity >= totalNeededQuantity && currentStatus != Order.OrderStatus.Collected)
        {
            order.Status = Order.OrderStatus.Collected;
            order.CollectedDateTime = DateTime.Now;
        }
    
        if (currentStatus != order.Status)
        {
            order.LastChangeDateTime = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}