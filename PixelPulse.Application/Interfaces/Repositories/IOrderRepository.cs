using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<Order> GetOrdersByUserIdAsync(string userId);
}