using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface ICartRepository : IBaseRepository<Cart>
{
    IQueryable<Cart> FindCartByUserId(string userId);
}