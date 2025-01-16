namespace PixelPulse.Domain.Entities;

public class Category
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public virtual Category ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Product> Products { get; set; }
}