using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IOrderItemRepository : IBaseRepository<OrderItem>
{
    Task<List<OrderItem>> GetOrderItemsByUserIdAsync(string userId);
    Task AddOrdersItemAsync(List<OrderItem> orderItems);
    Task RemoveOrdersItemAsync(List<OrderItem> orderItems);
}