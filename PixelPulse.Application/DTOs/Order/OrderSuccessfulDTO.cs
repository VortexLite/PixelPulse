using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.DTOs.Order;

public class OrderSuccessfulDTO
{
    public Domain.Entities.Shipping? Shipping { get; set; }
}