using PixelPulse.Application.DTOs.Cart;

namespace PixelPulse.Application.DTOs.Order;

public class CreateOrderDTO
{
    public List<CartItemDTO> CartItemDtos { get; set; }
    public string? PaymentMethod { get; set; }
    public string? City { get; set; }
    public string? Department { get; set; }
    
    public string? AppUserId { get; set; }
    public string? Email { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Phonenumber { get; set; }
}