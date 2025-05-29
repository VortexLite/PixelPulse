using Microsoft.EntityFrameworkCore;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Infrastructure.Content.Seeds;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Смартфони" },
            new Category { Id = 2, Name = "Ноутбуки" },
            new Category { Id = 3, Name = "Телевізори" },
            new Category { Id = 4, Name = "Аудіо" },
            new Category { Id = 5, Name = "Ігрові приставки" },
            new Category { Id = 6, Name = "Планшети" },
            new Category { Id = 7, Name = "Роутери" },
            new Category { Id = 8, Name = "Смарт-годинники" },
            new Category { Id = 9, Name = "Комп'ютери" },
            new Category { Id = 10, Name = "Аксесуари" }
        );

        modelBuilder.Entity<Discount>().HasData(
            Enumerable.Range(1, 10).Select(i => new Discount
            {
                Id = i,
                Code = $"DISCOUNT{i * 5}",
                Percentage = i * 5,
                ExpirationDate = DateTime.Now.AddMonths(i)
            }).ToArray()
        );

        modelBuilder.Entity<Setting>().HasData(
            new Setting { Id = 1, Key = "SiteTitle", Value = "Pixel Pulse" },
            new Setting { Id = 2, Key = "SupportEmail", Value = "support@pixelpulse.com" },
            new Setting { Id = 3, Key = "DefaultCurrency", Value = "UAH" },
            new Setting { Id = 4, Key = "ItemsPerPage", Value = "20" },
            new Setting { Id = 5, Key = "FreeShippingFrom", Value = "1000" }
        );

        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, Name = "Samsung Ukraine", ContactInfo = "samsung@ua.com" },
            new Supplier { Id = 2, Name = "Apple Inc.", ContactInfo = "apple@apple.com" },
            new Supplier { Id = 3, Name = "Xiaomi Global", ContactInfo = "contact@xiaomi.com" },
            new Supplier { Id = 4, Name = "HP Europe", ContactInfo = "support@hp.eu" },
            new Supplier { Id = 5, Name = "ASUS", ContactInfo = "info@asus.com" },
            new Supplier { Id = 6, Name = "Lenovo", ContactInfo = "info@lenovo.com" },
            new Supplier { Id = 7, Name = "Sony", ContactInfo = "service@sony.com" },
            new Supplier { Id = 8, Name = "LG Electronics", ContactInfo = "support@lg.com" },
            new Supplier { Id = 9, Name = "MSI", ContactInfo = "msi@support.com" },
            new Supplier { Id = 10, Name = "Canon", ContactInfo = "info@canon.com" }
        );
    }
}