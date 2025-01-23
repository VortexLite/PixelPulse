using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface ICartItemRepository : IBaseRepository<CartItem>
{
    Task AddItemsAsync(List<CartItem> cartItems);
    Task RemoveItemsAsync(List<CartItem> cartItems);
    Task<List<CartItem>> GetAllItemsByUserIdAsync(string userId);
}