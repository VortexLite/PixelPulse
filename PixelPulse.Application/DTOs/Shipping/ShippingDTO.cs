namespace PixelPulse.Application.DTOs.Shipping;

public class ShippingDTO
{
    public string TrackingNumber { get; set; }
    public DateTime ShippedDate { get; set; }
    public string Address { get; set; }
}