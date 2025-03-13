using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _cache;
    
    private const string _reviewCacheKey = "review_";

    public ReviewService(ILogger<ReviewService> logger, IUserService userService, IReviewRepository reviewRepository, IMemoryCache cache)
    {
        _logger = logger;
        _userService = userService;
        _reviewRepository = reviewRepository;
        _cache = cache;
    }
    
    public async Task<List<ReviewDTO>?> GetReviewByRefreshToken(string token)
    {
        var user = await _userService.FindUserByRefreshToken(token);
        if (!_cache.TryGetValue(_reviewCacheKey + "reviews" + $"{user.Id}", out List<ReviewDTO> cachedReviews))
        {
            var reviews = _reviewRepository.GetReviewsByUserId(user.Id);
            cachedReviews = await reviews
                .ProjectToType<ReviewDTO>()
                .ToListAsync();
            
            _cache.Set(_reviewCacheKey + "reviews" + $"{user.Id}", cachedReviews, TimeSpan.FromMinutes(30));
            foreach (var review in cachedReviews)
                _cache.Set(_reviewCacheKey + $"{review.Id}", review, TimeSpan.FromMinutes(30));
        }
        
        return cachedReviews;
        
        
    }
}