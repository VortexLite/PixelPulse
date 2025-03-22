namespace PixelPulse.Application.DTOs.Order;

public class OrderUpdateDTO
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    public List<OrderItemUpdate>? OrderItems { get; set; }
}