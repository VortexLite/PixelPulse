using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.DTOs.Order;

public class OrderControlDTO
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    public List<OrderItemControlDTO> OrderItems { get; set; }
}