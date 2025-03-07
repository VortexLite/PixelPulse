using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface ICartItemRepository : IBaseRepository<CartItem>
{   
    IQueryable<CartItem>? GetCartItemsByCartAndProductId(int cartId, int productId);
    IQueryable<CartItem>? GetCartItemsByCartId(int cartId);
    Task<bool> DeleteCartItemsAsync(List<CartItemDTO> cartItemDtos, CancellationToken cancellationToken = default); 
}