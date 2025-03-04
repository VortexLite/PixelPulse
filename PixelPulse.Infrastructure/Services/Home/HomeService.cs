using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Services.Home;

public class HomeService : IHomeService
{
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IAccountService _accountService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<HomeService> _logger;
    private readonly string _cacheProductKeyAll = "product_all";
    private readonly string _cacheProductKey = "product_";

    public HomeService(IProductRepository productRepository, ICartRepository cartRepository, ICartItemRepository cartItemRepository, IMemoryCache cache, ILogger<HomeService> logger, IAccountService accountService)
    {
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _cache = cache;
        _logger = logger;
        _accountService = accountService;
    }

    public async Task<List<ProductMainDTO>> GetAllProductWithImageTake20Async(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(_cacheProductKeyAll, out List<ProductMainDTO> cached))
        {
            _logger.LogInformation($"Product found: {_cacheProductKeyAll}");
            return cached;
        }
        else
        {
            cached = await _productRepository.GetAllProductWithImageTake20Async(cancellationToken);

            if (cached != null)
            {
                _logger.LogInformation($"Product founds");
                _cache.Set(_cacheProductKeyAll, cached, TimeSpan.FromMinutes(30));
                _logger.LogInformation($"Product cache created: {_cacheProductKeyAll}");
            }
            
            return cached;
        }
    }

    public async Task<bool> AddCartItemAsync(Dictionary<string, string> productAndToken,
        CancellationToken cancellationToken = default)
    {
        var user = await _accountService.FindUserByRefreshToken(productAndToken.GetValueOrDefault("refreshTokenKey"));
        if (user == null)
            return false;
        
        var cart = await _cartRepository.FindCartByUserIdAsync(user.Id, cancellationToken);
        if (cart == null)
        {
            cart = new Cart
            {
                AppUserId = productAndToken.GetValueOrDefault("refreshTokenKey"),
            };

            await _cartRepository.CreateAsync(cart, cancellationToken);
        }
        
        var cartItem = await _cartItemRepository.GetCartItemByCartAndProductId(cart.Id, Convert.ToInt32(productAndToken.GetValueOrDefault("productId")));
        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = Convert.ToInt32(productAndToken.GetValueOrDefault("productId")),
                Quantity = 1
            };
        
            await _cartItemRepository.CreateAsync(cartItem, cancellationToken);
        }
        else
        {
            cartItem.Quantity += 1;
        }
        
        await _cartItemRepository.UpdateAsync(cartItem, cancellationToken);
        
        return true;
    }
}