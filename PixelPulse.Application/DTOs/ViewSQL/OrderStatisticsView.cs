namespace PixelPulse.Application.DTOs.ViewSQL;

public class OrderStatisticsView
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
}