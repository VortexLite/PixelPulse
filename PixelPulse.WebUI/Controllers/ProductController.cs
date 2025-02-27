using Microsoft.AspNetCore.Mvc;
using PixelPulse.Infrastructure.Content.Data;

namespace PixelPulse.Controllers;

public class ProductController : Controller
{
    private readonly AppIdentityContext _context;
    private readonly ILogger<ProductController> _logger;

    public ProductController(AppIdentityContext context, ILogger<ProductController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Details(int id)
    {
        return View();
    }
}