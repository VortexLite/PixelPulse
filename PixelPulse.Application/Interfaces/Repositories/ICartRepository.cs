using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<Cart> GetCartByUserIdAsync(string userId);
}