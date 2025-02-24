namespace PixelPulse.Application.DTOs.Cart;

public class CartItemDTO
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public string ImageType { get; set; }
    public byte[] ImageData { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal OldPrice { get; set; }
    public decimal Price { get; set; }
}