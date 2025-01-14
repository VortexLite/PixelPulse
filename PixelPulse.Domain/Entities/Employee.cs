namespace PixelPulse.Domain.Entities;

public class Employee : BaseEntity
{
    private string AppUserId { get; set; }
    public AppUser Use { get; set; }
    public string Position { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
}