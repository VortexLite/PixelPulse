namespace PixelPulse.Application.DTOs.Order;

public class OrderItemDTO
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public string ImageType { get; set; }
    public byte[] ImageData { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}