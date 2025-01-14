namespace PixelPulse.Domain.Entities;

public class Review : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime ReviewDate { get; set; }
}