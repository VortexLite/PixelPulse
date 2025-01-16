namespace PixelPulse.Domain.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}