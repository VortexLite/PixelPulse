using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    IQueryable<Order>? GetOrdersByUserId(string userId);
}