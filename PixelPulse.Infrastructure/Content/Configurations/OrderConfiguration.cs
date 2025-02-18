using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.Property(o => o.TotalPrice)
            .HasColumnType("decimal(18, 2)");
        
        builder.HasOne(o => o.AppUser)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AppUserId);
        
    }
}