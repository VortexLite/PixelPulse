using Microsoft.AspNetCore.Mvc;
using PixelPulse.Application.DTOs.Order;
using PixelPulse.Application.DTOs.RefreshToken;
using PixelPulse.Application.Interfaces.Service;

namespace PixelPulse.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public OrderController(ILogger<OrderController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpPost("orderView")]
    public async Task<IActionResult> Order([FromBody] RefreshTokenRequest refreshToken)
    {
        return Ok(await _orderService.GetOrdersByRefreshToken(refreshToken.RefreshToken));
    }

    [HttpPost("createOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO сreateOrder)
    {
        if (await _orderService.CreateOrder(сreateOrder))
            return Ok();

        return BadRequest();
    }

    [HttpGet("cancelOrder/{orderId}")]
    public async Task<IActionResult> DellCart(int orderId)
    {
        if (await _orderService.CancelOrderAsync(orderId))
            return Ok();

        return BadRequest();
    }
}