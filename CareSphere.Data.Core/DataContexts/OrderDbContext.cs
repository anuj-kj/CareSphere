using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Domains.Orders;
using Microsoft.EntityFrameworkCore;

namespace CareSphere.Data.Core.DataContexts
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().HasMany(o => o.Items).WithOne().HasForeignKey(i => i.OrderId);
            modelBuilder.Entity<OrderItem>().HasKey(i => i.Id);
            modelBuilder.Entity<OrderItem>()
               .HasIndex(oi => new { oi.OrderId, oi.ProductId })
               .IsUnique();
        }
    }
}
