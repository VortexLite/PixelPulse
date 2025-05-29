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
    
    private const string _orderCacheKey = "order_";
    private const string _orderItemCacheKey = "orderItems_";

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
        
        if (!_cache.TryGetValue(_orderItemCacheKey + "view", out List<OrderDTO> cachedOrders))
        {
            var orders = _orderRepository
                .GetOrdersByUserId(user.Id);
        
            cachedOrders = await orders
                .ProjectToType<OrderDTO>()
                .ToListAsync();
            //cachedOrders = await _orderRepository.GetOrdersByUserId(user.Id);
            
            _cache.Set(_orderCacheKey + "view", cachedOrders, TimeSpan.FromMinutes(30));
            foreach (var cachedOrder in cachedOrders)
                _cache.Set(_orderItemCacheKey + "dto" + $"{cachedOrder.Id}", cachedOrder, TimeSpan.FromMinutes(30));
            
        }
        
        return cachedOrders;
    }
    
    public async Task<OrderSuccessfulDTO?> CreateOrder(CreateOrderDTO createOrder)
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
        
        _cache.Remove("orderItems_view");
        _cache.Set(_orderCacheKey + order.Id, order, TimeSpan.FromMinutes(30));
        
        return new OrderSuccessfulDTO { Shipping = shipping };
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
        
        _cache.Remove(_orderCacheKey + order.Id);
        
        return true;
    }

    public async Task<OrderAndCountDTO> GetrdersWithItemsTake50(int page)
    {
        var orders = await _orderRepository
            .GetOrderWithItems(page)
            .ProjectToType<OrderControlDTO>()
            .ToListAsync();
        
        int totalItems = await _orderRepository.CountAsync();
        var dto = new OrderAndCountDTO()
        {
            Orders = orders,
            CurrentPage = page,
            TotalPages = totalItems > 0 ? (int)Math.Ceiling((double)totalItems / 50) : 1,
            TotalItems = totalItems,
        };

        return dto;
    }

    public async Task<OrderUpdateDTO?> GetOrderById(int orderId)
    {
        return await _orderRepository.GetOrderItemsById(orderId)
            .ProjectToType<OrderUpdateDTO>()
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateOrder(OrderUpdateDTO orderUpdate)
    {
        if (orderUpdate == null)
            return false;

        var orders = orderUpdate.Adapt<Domain.Entities.Order>();
        return await _orderRepository.UpdateAsync(orders);
    }
}