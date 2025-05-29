using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Service;

public interface IOrderService
{
    Task<List<OrderDTO>?> GetOrdersByRefreshToken(string token);
    Task<OrderSuccessfulDTO> CreateOrder(CreateOrderDTO createOrder);
    Task<bool> CancelOrderAsync(int orderId);
    Task<OrderAndCountDTO> GetrdersWithItemsTake50(int page);
    Task<OrderUpdateDTO?> GetOrderById(int orderId);
    Task<bool> UpdateOrder(OrderUpdateDTO orderUpdate);
}