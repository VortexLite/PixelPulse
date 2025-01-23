using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IProductImageRepository : IBaseRepository<ProductImage>
{
    Task AddImagesAsync(List<ProductImage> productImages);
    Task RemoveImagesAsync(List<ProductImage> productImages);
    Task<List<ProductImage>> GetImagesByProductIdAsync(Guid productId);
}