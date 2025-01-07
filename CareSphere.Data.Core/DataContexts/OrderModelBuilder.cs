using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CareSphere.Data.Core.DataContexts
{
    public class OrderModelBuilder
    {
        public async Task BuildAsync(DbContextOptions<OrderDbContext> options)
        {           

            using (var context = new OrderDbContext(options))
            {

                await context.Database.EnsureCreatedAsync();
                //TODO: Add migration
            }

        }
    }
}
