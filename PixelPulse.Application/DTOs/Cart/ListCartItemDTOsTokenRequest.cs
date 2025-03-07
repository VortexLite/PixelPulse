namespace PixelPulse.Application.DTOs.Cart;

public class ListCartItemDTOsTokenRequest
{
    public string RefreshToken { get; set; }
    public List<CartItemDTO> CartItemDtos { get; set; }
}