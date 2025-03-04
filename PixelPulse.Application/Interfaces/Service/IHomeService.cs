using PixelPulse.Application.DTOs.Product;

namespace PixelPulse.Application.Interfaces.Service;

public interface IHomeService
{
    Task<List<ProductMainDTO>> GetAllProductWithImageTake20Async(CancellationToken cancellationToken = default);
    Task<bool> AddCartItemAsync(Dictionary<string, string> productAndToken, CancellationToken cancellationToken = default);
    Task<bool> AddWishlistItemAsync(Dictionary<string, string> productAndToken, CancellationToken cancellationToken = default);
}