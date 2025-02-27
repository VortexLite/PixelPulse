using PixelPulse.Application.DTOs.Shipping;

namespace PixelPulse.Application.DTOs.Order;

public class OrderDTO
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int TotalItems { get; set; }
    public int TotalPrice { get; set; }
    public string Status { get; set; }
    public ShippingDTO? Shipping { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
}