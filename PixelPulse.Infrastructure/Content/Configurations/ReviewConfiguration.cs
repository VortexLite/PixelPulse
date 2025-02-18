using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasOne(r => r.Product)
            .WithOne(p => p.Review)
            .HasForeignKey<Review>(r => r.ProductId);
        
        builder.HasOne(r => r.AppUser)
            .WithMany(a => a.Reviews)
            .HasForeignKey(r => r.AppUserId);
    }
}