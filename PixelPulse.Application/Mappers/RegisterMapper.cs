using Mapster;
using PixelPulse.Application.DTOs.Cart;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Application.DTOs.Review;
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
            .Map(dest => dest.TotalItems, src => src.OrderItems.Sum(oi => oi.Quantity))
            .Map(dest => dest.TotalPrice, src => src.OrderItems.Sum(oi => oi.Quantity * oi.Price))
            .Map(dest => dest.OrderItems, src => src.OrderItems.Adapt<List<OrderItemDTO>>());

        config.NewConfig<OrderItem, OrderItemDTO>()
            .Map(dest => dest.ProductName, src => src.Product.Name);
        
        config.NewConfig<CartItem, CartItemDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CartId, src => src.CartId)
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
    }
}