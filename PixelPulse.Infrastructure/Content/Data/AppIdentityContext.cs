using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Data;

public class AppIdentityContext(DbContextOptions<AppIdentityContext> options) : IdentityDbContext(options)
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppIdentityContext).Assembly);
    }
}