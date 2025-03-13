using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.Application.Service.Product;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IUserService _userService;
    private readonly IProductRepository _productRepository;
    private readonly IMemoryCache _cache;
    
    private const string _productCacheKey = "productItem_";

    public ProductService(ILogger<ProductService> logger, IUserService userService, IProductRepository productRepository, IMemoryCache cache)
    {
        _logger = logger;
        _userService = userService;
        _productRepository = productRepository;
        _cache = cache;
    }

    public async Task<List<ProductMainDTO>?> GetProductsWithImageTake20Async()
    {
        if (!_cache.TryGetValue(_productCacheKey + "products", out List<ProductMainDTO> cachedProducts))
        {
            var products = _productRepository.GetProductsWithImageTake20();
            cachedProducts = await products
                .ProjectToType<ProductMainDTO>()
                .ToListAsync();
            
            _cache.Set(_productCacheKey + "products", cachedProducts, TimeSpan.FromMinutes(30));
            foreach (var product in cachedProducts)
                _cache.Set(_productCacheKey + $"{product.Id}", product, TimeSpan.FromMinutes(30));
        }
        
        return cachedProducts;
    }

    public async Task<ProductCategoryDTO>? GetDetailsProduct(int productId)
    {
        if (!_cache.TryGetValue(_productCacheKey + "details" + $"{productId}", out ProductCategoryDTO cachedProducts))
        {
            var product = _productRepository.GetProductsWithImageAndCategoriesById(productId);
            cachedProducts = await product
                .ProjectToType<ProductCategoryDTO>()
                .FirstOrDefaultAsync();
            
            _cache.Set(_productCacheKey + "details" + $"{productId}", cachedProducts, TimeSpan.FromMinutes(30));
        }
        
        return cachedProducts;
    }
}