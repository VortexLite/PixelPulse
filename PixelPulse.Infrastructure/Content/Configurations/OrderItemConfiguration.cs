using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems").HasKey(k => k.OrderItemId);

        builder.Property(io => io.Price)
            .HasColumnType("decimal(18,2)");
        
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(oi => oi.ShippingAddress)
            .WithMany()
            .HasForeignKey(oi => oi.ShippingAddressId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(oi => oi.ShippingMethod)
            .WithMany()
            .HasForeignKey(oi => oi.ShippingMethodId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}