namespace PixelPulse.Domain.Entities;

public class Shipping
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public string TrackingNumber { get; set; }
    public DateTime ShippedDate { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
}