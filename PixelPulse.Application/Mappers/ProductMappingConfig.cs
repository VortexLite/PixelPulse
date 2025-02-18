using Mapster;
using PixelPulse.Application.DTOs.Product;
using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Mappers;

public class ProductMappingConfig : IRegister
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
    }
}