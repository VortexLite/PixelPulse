using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PixelPulse.Application.DTOs.Review;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.Application.Service.Review;

public class ReviewService : IReviewService
{
    private readonly ILogger<ReviewService> _logger;
    private readonly IUserService _userService;
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(ILogger<ReviewService> logger, IUserService userService, IReviewRepository reviewRepository)
    {
        _logger = logger;
        _userService = userService;
        _reviewRepository = reviewRepository;
    }
    
    public async Task<List<ReviewDTO>?> GetReviewByRefreshToken(string token)
    {
        var user = await _userService.FindUserByRefreshToken(token);
        var reviews = _reviewRepository.GetReviewsByUserId(user.Id);
        
        return await reviews
            .ProjectToType<ReviewDTO>()
            .ToListAsync();
    }
}