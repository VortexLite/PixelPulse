using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts").HasKey(k => k.CartId);
        
        builder.HasOne(c => c.AppUser)
            .WithMany(ap => ap.Carts)
            .HasForeignKey(c => c.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}