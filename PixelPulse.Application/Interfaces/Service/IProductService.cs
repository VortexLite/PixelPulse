using PixelPulse.Application.DTOs.Product;

namespace PixelPulse.Application.Interfaces.Service;

public interface IProductService
{
    Task<List<ProductMainDTO>?> GetProductsWithImageTake20Async();
    Task<ProductCategoryDTO>? GetDetailsProduct(int productId);
}