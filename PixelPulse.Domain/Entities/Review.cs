namespace PixelPulse.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}