using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IProductSpecificationRepository : IBaseRepository<ProductSpecification>
{
    Task<List<ProductSpecification>> GetByProductIdAsync(Guid productId);
}