using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Domains.Events;

namespace CareSphere.Domains.Orders
{
    public class OrderStatusChangedEvent : IDomainEvent
    {
        public Guid OrderId { get; }
        public OrderStatus OldStatus { get; }
        public OrderStatus NewStatus { get; }
        public DateTime OccurredOn { get; }

        public OrderStatusChangedEvent(Guid orderId, OrderStatus oldStatus, OrderStatus newStatus)
        {
            OrderId = orderId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
