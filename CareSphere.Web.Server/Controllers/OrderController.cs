using CareSphere.Services.Orders.Dtos;
using CareSphere.Services.Orders.Interfaces;
using CareSphere.Services.Organizations.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareSphere.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        [Authorize]
        [HttpGet("create")]

        public async Task<IActionResult> Create(List<OrderItemDto> orderItems)
        {
            try
            {
                await _orderService.CreateOrderAsync(orderItems);
                _logger.LogInformation("Order created successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during creating order");
                return NotFound("Error during creating order");
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync();
                _logger.LogInformation("Orders retrieved successfully");              
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during getting order");
                return NotFound("Error during getting order");
            }
        }
    }
}
