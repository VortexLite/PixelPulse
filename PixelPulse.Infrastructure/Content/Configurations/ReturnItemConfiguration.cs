using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class ReturnItemConfiguration : IEntityTypeConfiguration<ReturnItem>
{
    public void Configure(EntityTypeBuilder<ReturnItem> builder)
    {
        builder.ToTable("ReturnItems").HasKey(k => k.ReturnItemId);
        
        builder.HasOne(ri => ri.Return)
            .WithMany(r => r.ReturnItems)
            .HasForeignKey(ri => ri.ReturnId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ri => ri.OrderItem)
            .WithMany()
            .HasForeignKey(ri => ri.OrderItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}