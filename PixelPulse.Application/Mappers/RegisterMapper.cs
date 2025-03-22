using Mapster;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.DTOs.Review;
using PixelPulse.Application.DTOs.Shipping;
using PixelPulse.Application.DTOs.Wishlist;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Mappers;

public class RegisterMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductMainDTO>()
            .Map(dest => dest.ImagePath,
                src => src.ProductImages.FirstOrDefault(img => img.isMain).ImagePath ?? "wwwroot/images/image.webp");
        
        config.NewConfig<Order, OrderDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.OrderDate, src => src.OrderDate)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.TotalItems, src => src.OrderItems != null ? src.OrderItems.Count : 0)
            .Map(dest => dest.TotalPrice, src => src.TotalPrice)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.Shipping, src => src.Shipping.Adapt<ShippingDTO>())
            .Map(dest => dest.OrderItems, src => src.OrderItems.Adapt<List<OrderItemDTO>>());

        config.NewConfig<OrderItem, OrderItemDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.OrderId, src => src.OrderId)
            .Map(dest => dest.ProductName, src => src.Product != null ? src.Product.Name : "")
            .Map(dest => dest.ProductPrice, src => src.Product != null ? src.Product.Price : 0)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.TotalPrice, src => src.Price * src.Quantity)
            .Map(dest => dest.ImagePath, src => 
                src.Product != null && src.Product.ProductImages != null &&
                src.Product.ProductImages.Any(img => img.isMain)
                    ? src.Product.ProductImages.First(img => img.isMain).ImagePath 
                    : "wwwroot/images/image.webp");

        config.NewConfig<Shipping, ShippingDTO>()
            .Map(dest => dest.TrackingNumber, src => src.TrackingNumber)
            .Map(dest => dest.ShippedDate, src => src.ShippedDate);
        
        config.NewConfig<CartItem, CartItemDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CartId, src => src.CartId)
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.ProductName, src => src.Product.Name)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.OldPrice, src => src.Product.OldPrice)
            .Map(dest => dest.Price, src => src.Product.Price)
            .Map(dest => dest.ImagePath, src => 
                src.Product != null && src.Product.ProductImages != null &&
                src.Product.ProductImages.Any(img => img.isMain)
                    ? src.Product.ProductImages.First(img => img.isMain).ImagePath 
                    : "wwwroot/images/image.webp");
        
        config.NewConfig<WishlistItem, WishlistItemDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.WishlistId, src => src.WishlistId)
            .Map(dest => dest.ProductName, src => src.Product.Name)
            .Map(dest => dest.OldPrice, src => src.Product.OldPrice)
            .Map(dest => dest.Price, src => src.Product.Price)
            .Map(dest => dest.ImagePath, src => 
                src.Product != null && src.Product.ProductImages != null &&
                src.Product.ProductImages.Any(img => img.isMain)
                    ? src.Product.ProductImages.First(img => img.isMain).ImagePath 
                    : "wwwroot/images/image.webp");
        
        config.NewConfig<Review, ReviewDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ProductName, src => src.Product.Name)
            .Map(dest => dest.Comment, src => src.Comment ?? "No comment")
            .Map(dest => dest.Rating, src => src.Rating)
            .Map(dest => dest.ImagePath, src => 
                src.Product != null && src.Product.ProductImages != null &&
                src.Product.ProductImages.Any(img => img.isMain)
                    ? src.Product.ProductImages.First(img => img.isMain).ImagePath 
                    : "wwwroot/images/image.webp");
        
        config.NewConfig<Product, ProductCategoryDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.Stock, src => src.Stock)
            .Map(dest => dest.CategoryName, src => src.Category.Name)
            .Map(dest => dest.ProductImages, src => src.ProductImages ?? new List<ProductImage>())
            .Map(dest => dest.ProductSpecifications, src => src.ProductSpecifications.Adapt<List<ProductSpecificationDTO>>() ?? new List<ProductSpecificationDTO>());
    }
}