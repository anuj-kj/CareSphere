﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.DataContexts;

using CareSphere.Domains.Orders;

namespace CareSphere.Data.Orders.interfaces
{
    public class OrderRepository :  Repository<Order, OrderDbContext>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context) { }
       

        // Add any additional methods specific to Order repository
    }
}
