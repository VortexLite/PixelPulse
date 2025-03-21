using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IProductImageRepository : IBaseRepository<ProductImage>
{
    Task<bool> DeleteProductImages(List<ProductImage> productImages, CancellationToken cancellationToken = default);
    IQueryable<ProductImage> GetImagesByProductId(int productId);
}