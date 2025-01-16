using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class ProductBundleConfiguration : IEntityTypeConfiguration<ProductBundle>
{
    public void Configure(EntityTypeBuilder<ProductBundle> builder)
    {
        builder.ToTable("ProductBundles").HasKey(k => k.ProductBundleId);
        
        builder.Property(pb => pb.DiscountPercentage)
            .HasColumnType("decimal(3,2)");
    }
}