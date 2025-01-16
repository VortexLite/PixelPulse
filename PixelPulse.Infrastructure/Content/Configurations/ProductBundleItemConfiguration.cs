using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class ProductBundleItemConfiguration : IEntityTypeConfiguration<ProductBundleItem>
{
    public void Configure(EntityTypeBuilder<ProductBundleItem> builder)
    {
        builder.ToTable("ProductBundleItems").HasKey(k => k.ProductBundleItemId);
        
        builder.HasOne(bi => bi.Bundle)
            .WithMany(pb => pb.ProductBundleItems)
            .HasForeignKey(bi => bi.BundleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(bi => bi.Product)
            .WithMany()
            .HasForeignKey(bi => bi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}