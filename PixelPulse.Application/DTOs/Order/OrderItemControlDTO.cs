namespace PixelPulse.Application.DTOs.Order;

public class OrderItemControlDTO
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Domain.Entities.Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}