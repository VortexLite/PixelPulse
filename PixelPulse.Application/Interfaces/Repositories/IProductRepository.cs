using PixelPulse.Application.DTOs.Filter;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    IQueryable<Product>? GetProductByName(string name);
    Task<Product>? GetProductWithImageById(int id);
    IQueryable<Product> GetProductsWithImageAndCategoriesById(int id);
    IQueryable<Product> GetProductsWithImageTake20();
    Task<bool> DeleteRangeAsync(List<Product> products, CancellationToken cancellationToken = default);
    IQueryable<Product> GetFilteredProduct(FilterPhoneDTO filter);
    IQueryable<Product> GetProductTake50Async(int page);
    Task<int> CountAsync();

}