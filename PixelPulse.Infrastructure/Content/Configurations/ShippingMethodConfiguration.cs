using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class ShippingMethodConfiguration : IEntityTypeConfiguration<ShippingMethod>
{
    public void Configure(EntityTypeBuilder<ShippingMethod> builder)
    {
        builder.ToTable("ShippingMethods").HasKey(k => k.ShippingMethodId);

        builder.Property(sm => sm.Cost)
            .HasColumnType("decimal(18,2)");
        
        /*builder.HasOne(sm => sm.OrderItem)
            .WithOne(o => o.ShippingMethod)
            .HasForeignKey<ShippingMethod>(o => o.ShippingMethodId)
            .OnDelete(DeleteBehavior.Restrict);*/
    }
}