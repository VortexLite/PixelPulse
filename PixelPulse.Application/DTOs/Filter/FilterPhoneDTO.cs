using PixelPulse.Application.DTOs.Product;

namespace PixelPulse.Application.DTOs.Filter;

public class FilterPhoneDTO
{
    public string? SortBy { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<string>? Brands { get; set; }
    public List<string>? RAMSizes { get; set; }
    public List<string>? StorageSizes { get; set; }
    public List<string>? Processors { get; set; }
    public List<string> ScreenTypes { get; set; }
    
    public List<ProductMainDTO>? ProductMainDtos { get; set; }
}