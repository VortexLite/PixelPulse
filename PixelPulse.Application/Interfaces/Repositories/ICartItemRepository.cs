using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface ICartItemRepository : IBaseRepository<CartItem>
{   
    Task<CartItem?> GetCartItemByCartAndProductId(int cartId, int productId);
}