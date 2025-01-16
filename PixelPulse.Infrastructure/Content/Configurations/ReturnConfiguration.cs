using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Configurations;

public class ReturnConfiguration : IEntityTypeConfiguration<Return>
{
    public void Configure(EntityTypeBuilder<Return> builder)
    {
        builder.ToTable("Returns").HasKey(k => k.ReturnId);
        
        builder.HasOne(r => r.Order)
            .WithOne(o => o.Return)
            .HasForeignKey<Return>(r => r.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
            
    }
}