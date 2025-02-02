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
        public static IServiceCollection AddDataLayer(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDbContext)
        {
            // Add CareSphereDbContext with the provided configuration
            services.AddDbContext<CareSphereDbContext>(configureDbContext);

            // Add OrderDbContext with the provided configuration
            services.AddDbContext<OrderDbContext>(configureDbContext);

            // Register UnitOfWork for CareSphereDbContext
            services.AddScoped<IUnitOfWork, UnitOfWork<CareSphereDbContext>>();

            // Register UnitOfWork for OrderDbContext
            services.AddScoped<IUnitOfWork, UnitOfWork<OrderDbContext>>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
