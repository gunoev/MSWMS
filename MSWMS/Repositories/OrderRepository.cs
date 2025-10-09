namespace MSWMS.Repositories;

using MSWMS.Entities;
using Microsoft.EntityFrameworkCore;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByTransferOrderNumberAsync(string transferOrderNumber);
    Task<Order> UpdateAsync(Order order);
    Task DeleteAsync(int id);
}

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateAsync(Order order)
    {
        _context.Set<Order>().Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    /*public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Set<Order>()
            .Include(o => o.Items)
            .Include(o => o.Boxes)
            .Include(o => o.Scans)
            .Include(o => o.Users)
            .FirstOrDefaultAsync(o => o.Id == id);
    }*/
    
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Set<Order>()
            .Include(o => o.Items)
            .Include(o => o.Boxes)
            .Include(o => o.Scans)
            .Include(o => o.Users)
            .ToListAsync();
    }

    public async Task<Order?> GetByTransferOrderNumberAsync(string transferOrderNumber)
    {
        return await _context.Set<Order>()
            .Include(o => o.Items)
            .Include(o => o.Boxes)
            .Include(o => o.Scans)
            .Include(o => o.Users)
            .FirstOrDefaultAsync(o => o.TransferOrderNumber == transferOrderNumber);
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        _context.Set<Order>().Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _context.Set<Order>().FindAsync(id);
        if (order != null)
        {
            _context.Set<Order>().Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}