namespace PixelPulse.Domain.Entities;

public class Discount : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public double DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}