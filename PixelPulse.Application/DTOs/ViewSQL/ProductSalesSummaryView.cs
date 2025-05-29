namespace PixelPulse.Application.DTOs.ViewSQL;

public class ProductSalesSummaryView
{
    public string ProductName { get; set; }
    public int TotalQuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
}