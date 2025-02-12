using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace PixelPulse.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }
}