using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Domains.Events;

namespace CareSphere.Domains.Orders
{
    public class Order
    {
        public Guid Id { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public OrderStatus Status { get; private set; }
        private List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public Order()
        {
            Id = Guid.NewGuid();
            Items = new List<OrderItem>();
            Status = OrderStatus.Created;
        }

        public void AddItem(Guid orderId, Guid productId, int quantity, decimal price)
        {
            var item = new OrderItem(orderId, productId, quantity, price);
            Items.Add(item);
            _domainEvents.Add(new OrderItemAddedEvent(orderId, productId, quantity, price));
        }

        public void RemoveItem(Guid productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public void UpdateStatus(OrderStatus status)
        {
            if (Status != status)
            {
                var oldStatus = Status;
                Status = status;
                _domainEvents.Add(new OrderStatusChangedEvent(Id, oldStatus, status));
            }
        }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
