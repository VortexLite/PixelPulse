using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Filter;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Service.Product;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IProductImageRepository _productImageRepository;
    private readonly IFileService _fileService;
    private readonly IMemoryCache _cache;
    
    private const string _productCacheKey = "productItem_";

    public ProductService(ILogger<ProductService> logger, IProductRepository productRepository, IMemoryCache cache, IProductImageRepository productImageRepository, IFileService fileService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _cache = cache;
        _productImageRepository = productImageRepository;
        _fileService = fileService;
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

    public async Task<List<ProductMainDTO>?> GetProductsTake100()
    {
        if (!_cache.TryGetValue(_productCacheKey + "take100", out List<ProductMainDTO> cachedProducts))
        {
            var product = _productRepository.GetAll();
            cachedProducts = await product
                .ProjectToType<ProductMainDTO>()
                .ToListAsync();
            
            _cache.Set(_productCacheKey + "take100", cachedProducts, TimeSpan.FromMinutes(30));
        }
        
        return cachedProducts;
    }

    public async Task<List<ProductMainDTO>?> GetFilteredProduct(FilterPhoneDTO filter)
    {
        var productQuery = _productRepository.GetFilteredProduct(filter);
        return await productQuery
            .ProjectToType<ProductMainDTO>()
            .ToListAsync();
    }

    public async Task<ProductAndCountDTO> GetProductTake50(int page)
    {
        var products = await _productRepository
            .GetProductTake50Async(page)
            .ToListAsync();
        
        int totalItems = await _productRepository.CountAsync();
        var dto = new ProductAndCountDTO
        {
            Products = products,
            CurrentPage = page,
            TotalPages = totalItems > 0 ? (int)Math.Ceiling((double)totalItems / 50) : 1,
            TotalItems = totalItems,
        };

        return dto;
    }

    public async Task<bool> UpdateProductWithImage(ProductUpdateRequest request)
    {
        var product = await _productRepository.GetById(Convert.ToInt32(request.Id)).FirstOrDefaultAsync();
        if (product == null)
            return false;
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;
        product.SupplierId = request.SupplierId;

        if (request.ProductImages != null && request.ProductImages.Count > 0)
        {
            var imageList = new List<ProductImage>();
            foreach (var file in request.ProductImages)
            {
                var imagePath = await _fileService.SaveFileAsync(file, "images");
                imageList.Add(new ProductImage
                {
                    ProductId = product.Id, 
                    isMain = false,
                    ImagePath = imagePath
                });
            }
            
            product.ProductImages = imageList;
        }
        
        return await _productRepository.UpdateAsync(product);
    }

    public async Task<Domain.Entities.Product> GetProductById(int id)
    {
        return await _productRepository.GetById(id).FirstOrDefaultAsync();
    }

    public async Task<Domain.Entities.Product>? GetProducWithImageById(int productId)
    {
        var product = await _productRepository.GetById(productId).FirstOrDefaultAsync();
        var images = await _productImageRepository.GetImagesByProductId(productId).ToListAsync();
        product.ProductImages = images;
        
        return product;
    }

    public async Task<bool> DeleteProduct(int productId)
    {
        var product = await _productRepository.GetById(productId).FirstOrDefaultAsync();
        return await _productRepository.DeleteAsync(product);
    }
    
    public async Task<bool> MainProductImage(ProductImageIdDTO dto)
    {
        var image = await _productImageRepository.GetImagesByProductId(dto.ProductId)
            .Where(pi => pi.isMain == true).FirstOrDefaultAsync();
        if (image != null)
        {
            image.isMain = false;
            await _productImageRepository.UpdateAsync(image);
        }
        
        image = await _productImageRepository.GetById(dto.ImageId).FirstOrDefaultAsync();
        if (image != null)
        {
            image.isMain = true;
            await _productImageRepository.UpdateAsync(image);
        }
        
        return true;
    }

    public async Task<bool> DeleteProductImage(int imageId)
    {
        var productImage = await _productImageRepository.GetById(imageId).FirstOrDefaultAsync();
        return await _productImageRepository.DeleteAsync(productImage);
    }
}