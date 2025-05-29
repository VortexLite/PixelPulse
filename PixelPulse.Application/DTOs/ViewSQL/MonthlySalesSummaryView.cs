namespace PixelPulse.Application.DTOs.ViewSQL;

public class MonthlySalesSummaryView
{
    public string YearMonth { get; set; } // формат YYYY-MM
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}