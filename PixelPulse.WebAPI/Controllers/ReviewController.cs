using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.RefreshToken;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : Controller
{
    private readonly ILogger<ReviewController> _logger;
    private readonly IReviewService _reviewService;

    public ReviewController(ILogger<ReviewController> logger, IReviewService reviewService)
    {
        _logger = logger;
        _reviewService = reviewService;
    }
    
    [HttpPost("reviewView")]
    public async Task<IActionResult> Review([FromBody] RefreshTokenRequest refreshToken)
    {
        return Ok(await _reviewService.GetReviewByRefreshToken(refreshToken.RefreshToken));
    }
}