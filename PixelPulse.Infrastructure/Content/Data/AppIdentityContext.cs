using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PixelPulse.Application.DTOs.ViewSQL;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Configurations;

namespace PixelPulse.Infrastructure.Content.Data;

public class AppIdentityContext(DbContextOptions<AppIdentityContext> options) : IdentityDbContext(options)
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Shipping> Shippings { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<WishlistItem> WishlistItems { get; set; }
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }
    
    
    public DbSet<OrderStatisticsView> OrderStatisticsView { get; set; }
    public DbSet<MonthlySalesSummaryView> MonthlySalesSummaryViews { get; set; }
    public DbSet<ProductSalesSummaryView> ProductSalesSummaryViews { get; set; }
    public DbSet<TopSuppliersByRevenueView> TopSuppliersByRevenueViews { get; set; }
    public DbSet<UserWishlistStatsView> UserWishlistStatsViews { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppIdentityContext).Assembly);

        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new CartConfiguration());
        modelBuilder.ApplyConfiguration(new CartItemConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new SettingConfiguration());
        modelBuilder.ApplyConfiguration(new ShippingConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new WishlistConfiguration());
        modelBuilder.ApplyConfiguration(new WishlistItemConfiguration());
        modelBuilder.ApplyConfiguration(new ProductSpecificationConfiguration());
        
        modelBuilder
                .Entity<OrderStatisticsView>()
                .HasNoKey()
                .ToView("vw_OrderStatistics");
        
        modelBuilder.Entity<ProductSalesSummaryView>()
            .HasNoKey()
            .ToView("vw_ProductSalesSummary");
        
        modelBuilder.Entity<UserWishlistStatsView>()
            .HasNoKey()
            .ToView("vw_UserWishlistStats");
        
        modelBuilder.Entity<TopSuppliersByRevenueView>()
            .HasNoKey()
            .ToView("vw_TopSuppliersByRevenue");
        
        modelBuilder.Entity<MonthlySalesSummaryView>()
            .HasNoKey()
            .ToView("vw_MonthlySalesSummary");
    }
}