using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Domains.Orders;

namespace CareSphere.Services.Orders.Events.Handlers
{
    public class OrderEventHandlers
    {
        public void HandleOrderStatusChanged(OrderStatusChangedEvent domainEvent)
        {
            // Handle the event (e.g., send an email, update a read model, etc.)
            Console.WriteLine($"Order {domainEvent.OrderId} status changed from {domainEvent.OldStatus} to {domainEvent.NewStatus}.");
        }

        public void HandleOrderItemAdded(OrderItemAddedEvent domainEvent)
        {
            // Handle the event (e.g., update inventory, notify user, etc.)
            Console.WriteLine($"Item {domainEvent.ProductId} added to order {domainEvent.OrderId}.");
        }
    }
}
