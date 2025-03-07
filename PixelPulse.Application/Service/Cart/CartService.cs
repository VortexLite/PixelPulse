using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Service.Cart;

public class CartService : ICartService
{
    private readonly ILogger<CartService> _logger;
    private readonly IUserService _userService;
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMemoryCache _cache;
    
    private const string _cartCacheKey = "cartItems_";

    public CartService(ILogger<CartService> logger, IUserService userService, ICartRepository cartRepository, ICartItemRepository cartItemRepository, IMemoryCache cache)
    {
        _logger = logger;
        _userService = userService;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _cache = cache;
    }

    public async Task<List<CartItemDTO>?> GetCartsByRefreshTokenAsync(string token)
    {
        var user = await _userService.FindUserByRefreshToken(token);
        var cart = await _cartRepository.FindCartByUserId(user.Id)
            .Select(i => i.Id)
            .FirstOrDefaultAsync();

        if (!_cache.TryGetValue(_cartCacheKey + $"{cart}", out List<CartItemDTO> cachedCartItems))
        {
            var cartItemsQuery = _cartItemRepository.GetCartItemsByCartId(cart);
        
            var cartItems =  await cartItemsQuery
                .ProjectToType<CartItemDTO>()
                .ToListAsync();
        
            _cache.Set(_cartCacheKey + $"{cart}", cartItems, TimeSpan.FromMinutes(10));
        }
        
        return cachedCartItems;
        
    }

    public async Task<bool> DellCartItemAsync(int cartItemId)
    {
        var cartItem = await _cartItemRepository
            .GetById(cartItemId)
            .FirstOrDefaultAsync();
        
        if (cartItem == null)
            return false;
        
        await _cartItemRepository.DeleteAsync(cartItem);
        
        return true;
    }
    
    public async Task<bool> AddCartItemAsync(ProductIdTokenRequest request)
    {
        var user = await _userService.FindUserByRefreshToken(request.RefreshToken);
        if (user == null)
            return false;
        
        var cart = await _cartRepository.FindCartByUserId(user.Id).FirstOrDefaultAsync();
        if (cart == null)
        {
            cart = new Domain.Entities.Cart
            {
                AppUserId = user.Id,
            };

            await _cartRepository.CreateAsync(cart);
        }
        
        var cartItem = await _cartItemRepository
            .GetCartItemsByCartAndProductId(cart.Id, request.ProductId)
            .FirstOrDefaultAsync();
        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = 1
            };
        
            await _cartItemRepository.CreateAsync(cartItem);
        }
        else
        {
            cartItem.Quantity += 1;
        }
        
        await _cartItemRepository.UpdateAsync(cartItem);
        
        return true;
    }
}