using PixelPulse.Application.DTOs.ViewSQL;

namespace PixelPulse.Application.Interfaces.Service;

public interface IViewStatistic
{
    Task<List<MonthlySalesSummaryView>> GetMonthlySalesSummary();
    Task<List<OrderStatisticsView>> GetOrderStatistics();
    Task<List<ProductSalesSummaryView>> GetProductSalesSummary();
    Task<List<TopSuppliersByRevenueView>> GetTopSuppliersByRevenue();
    Task<List<UserWishlistStatsView>> GetUserWishlistStats();
}