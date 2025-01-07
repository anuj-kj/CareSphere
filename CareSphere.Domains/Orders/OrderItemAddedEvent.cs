
using CareSphere.Domains.Events;

namespace CareSphere.Domains.Orders
{
    public class OrderItemAddedEvent : IDomainEvent
    {
        public Guid OrderId { get; }
        public Guid ProductId { get; }
        public int Quantity { get; }
        public decimal Price { get; }
        public DateTime OccurredOn { get; }

        public OrderItemAddedEvent(Guid orderId, Guid productId, int quantity, decimal price)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
