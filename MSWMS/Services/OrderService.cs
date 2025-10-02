using MSWMS.Entities;
using MSWMS.Repositories;

namespace MSWMS.Services;

public class OrderService
{
    private IOrderRepository _orderRepository;
    
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }
}