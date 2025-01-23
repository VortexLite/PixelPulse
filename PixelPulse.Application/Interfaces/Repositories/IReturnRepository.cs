using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IReturnRepository : IBaseRepository<Return>
{
    Task<Return> GetByOrderIdAsync(Guid orderId);
}