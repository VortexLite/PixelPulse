using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryController : ControllerBase
{
    private readonly ILogger<DeliveryController> _logger;
    private readonly IDeliveryeService _deliveryeService;

    public DeliveryController(ILogger<DeliveryController> logger, IDeliveryeService deliveryeService)
    {
        _logger = logger;
        _deliveryeService = deliveryeService;
    }
    
    [HttpGet("GetCities")]
    public async Task<IActionResult> GetCities(string search)
    {
        var cities = await _deliveryeService.SearchCitiesAsync(search);
        return Ok(cities);
    }

    [HttpGet("GetDepartments")]
    public async Task<IActionResult> GetDepartments(string cityRef)
    {
        var departments = await _deliveryeService.GetDepartmentsAsync(cityRef);
        return Ok(departments);
    }
}