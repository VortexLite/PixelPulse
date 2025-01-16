using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.ToTable("Wishlists").HasKey(k => k.WishlistId);
        
        builder.HasOne(w => w.AppUser)
            .WithMany(c => c.Wishlists)
            .HasForeignKey(w => w.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}