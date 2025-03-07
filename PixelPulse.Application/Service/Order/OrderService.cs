using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Service.Order;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IUserService _userService;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(ILogger<OrderService> logger, IUserService userService, IOrderRepository orderRepository, IProductRepository productRepository, IOrderItemRepository orderItemRepository)
    {
        _logger = logger;
        _userService = userService;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _orderItemRepository = orderItemRepository;
    }

    public async Task<List<OrderDTO>?> GetOrdersByRefreshToken(string token)
    {
        var user = await _userService.FindUserByRefreshToken(token);
        if (user == null)
            return null;

        var orders = _orderRepository
            .GetOrdersByUserId(user.Id);
        
        return await orders
            .ProjectToType<OrderDTO>()
            .ToListAsync();
    }

    public async Task<bool> CreateOrder(ListCartItemDTOsTokenRequest cartItemDtosTokenRequest)
    {
        var user = await _userService.FindUserByRefreshToken(cartItemDtosTokenRequest.RefreshToken);
        if (user == null)
            return false;

        var order = new Domain.Entities.Order
        {
            AppUserId = user.Id
        };
        await _orderRepository.CreateAsync(order);
        
        var orderItems = new List<OrderItem>();
        foreach (var cartItem in cartItemDtosTokenRequest.CartItemDtos)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = _productRepository
                    .GetProductByName(cartItem.ProductName)
                    .FirstOrDefaultAsync().Id,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            };
            
            orderItems.Add(orderItem);
        }
        
        await _orderItemRepository.AddRangeAsync(orderItems);

        return true;
    }

    public async Task<List<ProductMainDTO>> GetAllProductWithImageTake20Async()
    {
        var products = _productRepository.GetProductsWithImageTake20();
        return await products
            .ProjectToType<ProductMainDTO>()
            .ToListAsync();
    }
}