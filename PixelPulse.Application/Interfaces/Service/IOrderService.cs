using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Service;

public interface IOrderService
{
    Task<List<OrderDTO>?> GetOrdersByRefreshToken(string token);
    Task<Order?> CreateOrder(CreateOrderDTO createOrder);
    Task<bool> CancelOrderAsync(int orderId);
}