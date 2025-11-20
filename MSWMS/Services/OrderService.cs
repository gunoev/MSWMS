using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Repositories;

namespace MSWMS.Services;

public class OrderService
{
    private IOrderRepository _orderRepository;
    private readonly AppDbContext _context;
    
    public OrderService(IOrderRepository orderRepository, AppDbContext context)
    {
        _orderRepository = orderRepository;
        _context = context;
    }
    
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
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