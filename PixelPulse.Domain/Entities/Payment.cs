namespace PixelPulse.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;
}