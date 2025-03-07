using Mapster;
using Microsoft.EntityFrameworkCore;
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

    public ProductService(ILogger<ProductService> logger, IUserService userService, IProductRepository productRepository)
    {
        _logger = logger;
        _userService = userService;
        _productRepository = productRepository;
    }

    public async Task<List<ProductMainDTO>?> GetProductsWithImageTake20Async()
    {
        var products = _productRepository.GetProductsWithImageTake20();
        return await products
            .ProjectToType<ProductMainDTO>()
            .ToListAsync();
    }

    public async Task<ProductCategoryDTO>? GetDetailsProduct(int productId)
    {
        var product = _productRepository.GetProductsWithImageAndCategoriesById(productId);
        return await product
            .ProjectToType<ProductCategoryDTO>()
            .FirstOrDefaultAsync();
    }
}