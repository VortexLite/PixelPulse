using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ViewStatisticController : ControllerBase
{
    private readonly ILogger<ViewStatisticController> _logger;
    private readonly IViewStatistic _viewStatistic;

    public ViewStatisticController(ILogger<ViewStatisticController> logger, IViewStatistic viewStatistic)
    {
        _logger = logger;
        _viewStatistic = viewStatistic;
    }
    
    [HttpGet("MonthySalesSummaryView")]
    public async Task<IActionResult> MonthySalesSummaryView()
    {
        return Ok(await _viewStatistic.GetMonthlySalesSummary());
    }
    
    [HttpGet("OrderStatisticsView")]
    public async Task<IActionResult> OrderStatisticsView()
    {
        return Ok(await _viewStatistic.GetOrderStatistics());
    }
    
    [HttpGet("ProductSalesSummaryView")]
    public async Task<IActionResult> ProductSalesSummaryView()
    {
        return Ok(await _viewStatistic.GetProductSalesSummary());
    }
    
    [HttpGet("TopSuppliersByRevenueView")]
    public async Task<IActionResult> TopSuppliersByRevenueView()
    {
        return Ok(await _viewStatistic.GetTopSuppliersByRevenue());
    }
    
    [HttpGet("UserWishlistStatsView")]
    public async Task<IActionResult> UserWishlistStatsView()
    {
        return Ok(await _viewStatistic.GetUserWishlistStats());
    }
}