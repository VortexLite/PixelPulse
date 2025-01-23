using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category> GetCategoryByNameAsync(string name);
    Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync();
}