using PixelPulse.Domain.Entities;

namespace PixelPulse.Application.Interfaces.Repositories;

public interface IShippingAddressRepository : IBaseRepository<ShippingAddress>
{
    Task<ShippingAddress?> GetAddressByUserIdAsync(string userId);
}