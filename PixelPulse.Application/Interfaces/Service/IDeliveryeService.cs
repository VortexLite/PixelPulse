using PixelPulse.Application.DTOs.NovaPoshta;

namespace PixelPulse.Application.Interfaces.Service;

public interface IDeliveryeService
{
    Task<List<City>> GetCitiesAsync();
    Task<List<City>> SearchCitiesAsync(string searchQuery);
    Task<List<Department>> GetDepartmentsAsync(string cityRef);
}