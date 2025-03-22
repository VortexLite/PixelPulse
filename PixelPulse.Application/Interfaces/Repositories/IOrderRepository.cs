using PixelPulse.Application.DTOs.Order;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    IQueryable<Order> GetOrdersByUserId(string userId);
    IQueryable<Order> GetOrderWithItems(int page);
    Task<int> CountAsync();
    IQueryable<Order> GetOrderItemsById(int orderId);
}