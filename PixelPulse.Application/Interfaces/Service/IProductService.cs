using PixelPulse.Application.DTOs.Filter;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Service;

public interface IProductService
{
    Task<List<ProductMainDTO>?> GetProductsWithImageTake20Async();
    Task<ProductCategoryDTO>? GetDetailsProduct(int productId);
    Task<List<ProductMainDTO>?> GetProductsTake100();
    Task<List<ProductMainDTO>?> GetFilteredProduct(FilterPhoneDTO filter);
    Task<ProductAndCountDTO> GetProductTake50(int page);
    Task<bool> UpdateProductWithImage(ProductUpdateRequest request);
    Task<Product> GetProductById(int id);
    Task<Product>? GetProducWithImageById(int productId);
    Task<bool> DeleteProduct(int productId);
    Task<bool> MainProductImage(ProductImageIdDTO dto);
    Task<bool> DeleteProductImage(int imageId);
    Task<bool> CreateProductWithImage(ProductUpdateRequest request);
}