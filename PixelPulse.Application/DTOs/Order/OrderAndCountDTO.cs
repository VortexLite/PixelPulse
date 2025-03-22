namespace PixelPulse.Application.DTOs.Order;

public class OrderAndCountDTO
{
    public List<OrderControlDTO> Orders { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}