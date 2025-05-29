
using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.DTOs.ViewSQL;
using PixelPulse.Application.Interfaces.Service;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Infrastructure.Services.ViewStatistic;

public class ViewStatistic : IViewStatistic
{
    private readonly AppIdentityContext _dbContext;

    public ViewStatistic(AppIdentityContext appIdentityContext)
    {
        _dbContext = appIdentityContext;
    }

    public async Task<List<MonthlySalesSummaryView>> GetMonthlySalesSummary()
    {
        return await _dbContext.MonthlySalesSummaryViews.ToListAsync();
    }

    public async Task<List<OrderStatisticsView>> GetOrderStatistics()
    {
        return await _dbContext.OrderStatisticsView.ToListAsync();
    }

    public async Task<List<ProductSalesSummaryView>> GetProductSalesSummary()
    {
        return await _dbContext.ProductSalesSummaryViews.ToListAsync();
    }

    public async Task<List<TopSuppliersByRevenueView>> GetTopSuppliersByRevenue()
    {
        return await _dbContext.TopSuppliersByRevenueViews.ToListAsync();
    }

    public async Task<List<UserWishlistStatsView>> GetUserWishlistStats()
    {
        return await _dbContext.UserWishlistStatsViews.ToListAsync();
    }
}