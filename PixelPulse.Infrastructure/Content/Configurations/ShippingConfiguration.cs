using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class ShippingConfiguration : IEntityTypeConfiguration<Shipping>
{
    public void Configure(EntityTypeBuilder<Shipping> builder)
    {
        builder.ToTable("Shipping");
        
        builder.HasOne(sh => sh.Order)
            .WithOne(a => a.Shipping)
            .HasForeignKey<Shipping>(sh => sh.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        builder.HasOne(sh => sh.Address)
            .WithMany(a => a.Shippings)
            .HasForeignKey(a => a.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}