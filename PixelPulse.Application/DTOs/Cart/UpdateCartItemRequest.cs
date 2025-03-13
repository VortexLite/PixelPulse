namespace PixelPulse.Application.DTOs.Cart;

public class UpdateCartItemRequest
{
    public int CartItemId { get; set; }
    public int Quantity { get; set; }
}