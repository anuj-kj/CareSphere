using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.Interfaces;
using CareSphere.Data.Orders.interfaces;
using CareSphere.Domains.Events;
using CareSphere.Domains.Orders;
using CareSphere.Services.Orders.Dtos;
using CareSphere.Services.Orders.Interfaces;

namespace CareSphere.Services.Orders.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public OrderService(IOrderRepository orderRepository, IDomainEventPublisher domainEventPublisher, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _domainEventPublisher = domainEventPublisher;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateOrderAsync(List<OrderItemDto> orderItems)
        {
            try
            {
                var order = new Order();
                foreach (var item in orderItems)
                {
                    order.AddItem(order.Id, item.ProductId, item.Quantity, item.Price);
                }
                await _orderRepository.AddAsync(order);
                await _unitOfWork.CommitAsync();

                foreach (var domainEvent in order.DomainEvents)
                {
                    _domainEventPublisher.Publish(domainEvent);
                }

                order.ClearDomainEvents();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception("Error creating order", ex);
            }
        }

        public async Task<Order> GetOrderAsync(Guid orderId)
        {
            return await _orderRepository.GetByGuidIdAsync(orderId);
        }
        public async Task<List<Order>> GetOrdersAsync()
        {
            var orers= await _orderRepository.GetAllAsync();
            return orers.ToList();
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            try
            {
                var order = await _orderRepository.GetByGuidIdAsync(orderId);
                order.UpdateStatus(status);
                _orderRepository.Update(order);
                await _unitOfWork.CommitAsync();
                foreach (var domainEvent in order.DomainEvents)
                {
                    _domainEventPublisher.Publish(domainEvent);
                }
                order.ClearDomainEvents();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();            
                throw new Exception("Error updating order status", ex);
            }
           
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByGuidIdAsync(orderId);
            _orderRepository.Delete(order);
        }
    }
}
