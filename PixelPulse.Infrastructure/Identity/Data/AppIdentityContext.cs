using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PixelPulse.Infrastructure.Identity.Data;

public class AppIdentityContext : IdentityDbContext
{
    public AppIdentityContext(DbContextOptions<AppIdentityContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppIdentityContext).Assembly);
    }
}