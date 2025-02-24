using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.DTOs.Order;

public class OrderDTO
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public Payment? Payment { get; set; }
    public Shipping? Shipping { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
    public int TotalItems { get; set; } 
    public decimal TotalPrice { get; set; } 
}