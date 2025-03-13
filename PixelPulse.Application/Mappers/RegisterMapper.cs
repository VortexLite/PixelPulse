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
            .Map(dest => dest.ImageData, 
                src => src.ProductImages.FirstOrDefault(img => img.isMain) != null 
                    ? src.ProductImages.FirstOrDefault(img => img.isMain)!.ImageData 
                    : null)
            .Map(dest => dest.ImageType, 
                src => src.ProductImages.FirstOrDefault(img => img.isMain) != null 
                    ? src.ProductImages.FirstOrDefault(img => img.isMain)!.ImageType 
                    : null);
        
        config.NewConfig<Order, OrderDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.OrderDate, src => src.OrderDate)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.TotalItems, src => src.OrderItems.Sum(oi => oi.Quantity))
            .Map(dest => dest.TotalPrice, src => src.OrderItems.Sum(item => item.Price * item.Quantity))
            .Map(dest => dest.Shipping, src => src.Shipping.Adapt<ShippingDTO>())
            .Map(dest => dest.OrderItems, src => src.OrderItems.Adapt<List<OrderItemDTO>>());

        config.NewConfig<OrderItem, OrderItemDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.OrderId, src => src.OrderId)
            .Map(dest => dest.ProductName, src => src.Product.Name)
            .Map(dest => dest.ProductPrice, src => src.Product.Price)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.TotalPrice, src => src.Price * src.Quantity)
            .Map(dest => dest.ImageType, 
                src => src.Product.ProductImages
                    .Where(img => img.isMain)
                    .Select(img => img.ImageType)
                    .FirstOrDefault())
            .Map(dest => dest.ImageData, 
                src => src.Product.ProductImages
                    .Where(img => img.isMain)
                    .Select(img => img.ImageData)
                    .FirstOrDefault());

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
            .Map(dest => dest.ImageType, 
                src => src.Product.ProductImages
                    .Where(img => img.isMain)
                    .Select(img => img.ImageType)
                    .FirstOrDefault())
            .Map(dest => dest.ImageData, 
                src => src.Product.ProductImages
                    .Where(img => img.isMain)
                    .Select(img => img.ImageData)
                    .FirstOrDefault());
        
        config.NewConfig<WishlistItem, WishlistItemDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.WishlistId, src => src.WishlistId)
            .Map(dest => dest.ProductName, src => src.Product.Name)
            .Map(dest => dest.OldPrice, src => src.Product.OldPrice)
            .Map(dest => dest.Price, src => src.Product.Price)
            .Map(dest => dest.ImageType, 
                src => src.Product.ProductImages
                    .Where(img => img.isMain)
                    .Select(img => img.ImageType)
                    .FirstOrDefault())
            .Map(dest => dest.ImageData, 
                src => src.Product.ProductImages
                    .Where(img => img.isMain)
                    .Select(img => img.ImageData)
                    .FirstOrDefault());
        
        config.NewConfig<Review, ReviewDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ProductName, src => src.Product.Name)
            .Map(dest => dest.Comment, src => src.Comment ?? "No comment")
            .Map(dest => dest.Rating, src => src.Rating)
            .Map(dest => dest.ImageType, 
                src => src.Product.ProductImages
                    .Where(img => img.isMain)
                    .Select(img => img.ImageType)
                    .FirstOrDefault())
            .Map(dest => dest.ImageData, 
                src => src.Product.ProductImages
                    .Where(img => img.isMain)
                    .Select(img => img.ImageData)
                    .FirstOrDefault());
        
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