namespace PixelPulse.Domain.Entities;

public class Return
{
    public Guid ReturnId { get; set; }
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; }
    public string Reason { get; set; }
    public string ReturnStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<ReturnItem> ReturnItems { get; set; }
}