using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.Interfaces;
using CareSphere.Domains.Orders;

namespace CareSphere.Data.Orders.interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
    }
}
