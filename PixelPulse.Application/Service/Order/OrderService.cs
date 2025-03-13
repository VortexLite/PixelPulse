using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Application.Service.OtherService;
using PixelPulse.Domain.Entities;
using PixelPulse.Domain.Enums;

namespace PixelPulse.Application.Service.Order;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IUserService _userService;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IShippingRepository _shippingRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IMemoryCache _cache;
    
    private const string _orderCacheKey = "orderItems_";

    public OrderService(ILogger<OrderService> logger, IUserService userService, IOrderRepository orderRepository, IProductRepository productRepository, IOrderItemRepository orderItemRepository, IMemoryCache cache, IPaymentRepository paymentRepository, IShippingRepository shippingRepository, ICartItemRepository cartItemRepository, ICartRepository cartRepository)
    {
        _logger = logger;
        _userService = userService;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _orderItemRepository = orderItemRepository;
        _cache = cache;
        _paymentRepository = paymentRepository;
        _shippingRepository = shippingRepository;
        _cartItemRepository = cartItemRepository;
        _cartRepository = cartRepository;
    }

    public async Task<List<OrderDTO>?> GetOrdersByRefreshToken(string token)
    {
        var user = await _userService.FindUserByRefreshToken(token);
        if (user == null)
            return null;

        if (!_cache.TryGetValue(_orderCacheKey + "orders" + $"{user.Id}", out List<OrderDTO> cachedOrders))
        {
            var orders = _orderRepository
                .GetOrdersByUserId(user.Id);
        
            cachedOrders = await orders
                .ProjectToType<OrderDTO>()
                .ToListAsync();
            
            _cache.Set(_orderCacheKey + "orders" + $"{user.Id}", cachedOrders, TimeSpan.FromMinutes(30));
            foreach (var cachedOrder in cachedOrders)
                _cache.Set(_orderCacheKey + "dto" + $"{cachedOrder.Id}", cachedOrder, TimeSpan.FromMinutes(30));
            
        }
        
        return cachedOrders;
    }
    
    public async Task<Domain.Entities.Order?> CreateOrder(CreateOrderDTO createOrder)
    {
        var totalPrice = createOrder.CartItemDtos.Sum(ci => ci.Price);
        
        var order = new Domain.Entities.Order
        {
            AppUserId = createOrder.AppUserId,
            TotalPrice = totalPrice,
            Status = OrderStatus.Pending.ToString(),
        };
        if (!await _orderRepository.CreateAsync(order))
            return null;

        var payment = new Payment
        {
            OrderId = order.Id,
            Amount = totalPrice,
            PaymentMethod = createOrder.PaymentMethod
        };
        if (!await _paymentRepository.CreateAsync(payment))
            return null;

        var shipping = new Shipping
        {
            OrderId = order.Id,
            TrackingNumber = TrackingNumberService.GenerateTrackingNumber(),
            ShippedDate = DateTime.UtcNow.AddDays(5),
            City = createOrder.City,
            Department = createOrder.Department
        };
        if (!await _shippingRepository.CreateAsync(shipping))
            return null;

        var orderItems = createOrder.CartItemDtos
            .Select(cartItemDto => new OrderItem
            {
                OrderId = order.Id, 
                ProductId = cartItemDto.ProductId, 
                Quantity = cartItemDto.Quantity, 
                Price = cartItemDto.Price
            }).ToList();
        if (!await _orderItemRepository.AddRangeAsync(orderItems))
            return null;
        
        var cart = await _cartRepository.FindCartByUserId(createOrder.AppUserId).FirstOrDefaultAsync();
        var cartItems = await _cartItemRepository.GetCartItemsByCartId(cart.Id).ToListAsync();

        foreach (var cartItem in cartItems)
            await _cartItemRepository.DeleteAsync(cartItem);
        
        if (!await _cartRepository.DeleteAsync(cart))
            return null;
        
        return order;
    }

    public async Task<bool> CancelOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetById(orderId).FirstOrDefaultAsync();
        if (order == null ||
            order.Status == OrderStatus.Cancelled.ToString())
            return false;
        
        var orderItems = await _orderItemRepository
            .GetOrderItemsByOrderId(orderId)
            .ToListAsync();
        
        if (orderItems.Count == 0)
            return false;

        foreach (var orderItem in orderItems)
        {
            var product = await _productRepository.GetById(orderItem.ProductId).FirstOrDefaultAsync();
            if (product != null)
            {
                product.Stock += orderItem.Quantity;
                await _productRepository.UpdateAsync(product);
            }
        }
        
        order.Status = OrderStatus.Cancelled.ToString();
        await _orderRepository.UpdateAsync(order);
        
        return true;
    }
}