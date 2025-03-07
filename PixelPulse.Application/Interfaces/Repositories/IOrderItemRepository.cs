using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IOrderItemRepository : IBaseRepository<OrderItem>
{
    Task<bool> AddRangeAsync(List<OrderItem> orderItems, CancellationToken cancellationToken = default);
}