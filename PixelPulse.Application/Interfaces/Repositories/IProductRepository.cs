using PixelPulse.Application.DTOs.Product;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> GetProductByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Product?> GetProductWithImageByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductCategoryDTO> GetProductWithImageAndCategoriesByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<ProductMainDTO>> GetAllProductWithImageTake20Async(CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(List<Product> products, CancellationToken cancellationToken = default);

}