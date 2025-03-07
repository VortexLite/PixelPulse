using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    IQueryable<Product>? GetProductByName(string name);
    IQueryable<Product>? GetProductWithImageById(int id);
    IQueryable<Product> GetProductsWithImageAndCategoriesById(int id);
    IQueryable<Product> GetProductsWithImageTake20();
    Task<bool> DeleteRangeAsync(List<Product> products, CancellationToken cancellationToken = default);

}