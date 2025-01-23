using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IReturnItemRepository : IBaseRepository<ReturnItem>
{
    Task AddReturnItemsAsync(List<ReturnItem> returnItems);
    Task RemoveReturnItemsAsync(List<ReturnItem> returnItems);
    Task<List<ReturnItem>> GetReturnItemsByUserIdAsync(string userId);
    Task<List<ReturnItem>> GetReturnItemsByReturnIdAsync(Guid returnId);
}