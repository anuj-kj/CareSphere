using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareSphere.Domains.Orders
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }

        public OrderItem(Guid orderId, Guid productId, int quantity, decimal price)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void UpdatePrice(decimal price)
        {
            Price = price;
        }
    }
}
