namespace PixelPulse.Domain.Entities;

public class Coupon
{
    public Guid CouponId { get; set; }
    public string Code { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}