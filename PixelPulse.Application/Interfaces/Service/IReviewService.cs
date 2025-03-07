using PixelPulse.Application.DTOs.Review;

namespace PixelPulse.Application.Interfaces.Service;

public interface IReviewService
{
    Task<List<ReviewDTO>?> GetReviewByRefreshToken(string token);
}