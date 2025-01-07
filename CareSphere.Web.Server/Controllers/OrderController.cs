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
        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpGet("create")]

        public async Task<IActionResult> Create(List<OrderItemDto> orderItems)
        {
            try
            {
                await _orderService.CreateOrderAsync(orderItems);
                return Ok();
            }
            catch (Exception ex)
            {
                //Log the error i.e., ex.Message
                return NotFound("Error during creating order");
            }
        }
    }
}
