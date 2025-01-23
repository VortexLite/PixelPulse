using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IReviewRepository : IBaseRepository<Review>
{
    Task<List<Review>> GetReviewByUserIdAsync(string userId);
}