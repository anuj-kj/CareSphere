using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Domains.Core;
using Microsoft.EntityFrameworkCore;

namespace CareSphere.Data.Core.DataContexts
{
    public class CareSphereDbContext : DbContext
    {
        public CareSphereDbContext(DbContextOptions<CareSphereDbContext> options) : base(options) { }

        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<User> User { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    // Apply additional configurations here
        //}
    }
}
