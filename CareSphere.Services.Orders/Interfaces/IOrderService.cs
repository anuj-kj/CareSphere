using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Domains.Orders;
using CareSphere.Services.Orders.Dtos;

namespace CareSphere.Services.Orders.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(List<OrderItemDto> orderItems);
        Task<Order> GetOrderAsync(Guid orderId);
        Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        Task DeleteOrderAsync(Guid orderId);
         Task<List<Order>> GetOrdersAsync();
    }
}
