using CareSphere.Data.Core.DataContexts;
using CareSphere.Data.Core.Impl;
using CareSphere.Data.Core.Interfaces;
using CareSphere.Data.Orders.interfaces;
using CareSphere.Data.Organaizations.Impl;
using CareSphere.Data.Organaizations.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CareSphere.Data.Configurations
{
    public static class DataLayerExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, string? connectionString)
        {
            // Add CareSphereDbContext
            services.AddDbContext<CareSphereDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add OrderDbContext
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
           

            return services;
        }
    }
}
