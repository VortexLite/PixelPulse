using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IReviewRepository : IBaseRepository<Review>
{
    IQueryable<Review> GetReviewsByUserId(string userId);
}