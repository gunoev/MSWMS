using MSWMS.Entities;
using MSWMS.Repositories;

namespace MSWMS.Services;

public class OrderService
{
    private IOrderRepository _orderRepository;
    public async Task<Order?> GetByIdAsync(int id)
    {
        _orderRepository = new OrderRepository(new AppDbContext());
        
        return _orderRepository.GetByIdAsync(id).Result;
    }
}