using PixelPulse.Application.DTOs.Product;

namespace PixelPulse.Application.Interfaces.Service;

public interface IHomeService
{
    Task<List<ProductMainDTO>> GetAllProductWithImageTake20Async(CancellationToken cancellationToken = default);
    Task<bool> AddCartItemAsync(Dictionary<string, string> ProductAndToken, CancellationToken cancellationToken = default);
}