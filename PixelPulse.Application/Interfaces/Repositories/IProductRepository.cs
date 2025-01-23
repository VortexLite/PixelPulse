using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product> GetProductByNameAsync(string name);
    Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
}