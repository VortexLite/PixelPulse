using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{

    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");

        builder.HasOne(pi => pi.Product)
            .WithMany(p => p.ProductImages);
    }
}