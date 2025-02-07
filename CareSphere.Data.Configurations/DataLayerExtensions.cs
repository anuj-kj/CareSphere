using CareSphere.Data.Core.DataContexts;

using CareSphere.Data.Core.Interfaces;
using CareSphere.Data.Orders.interfaces;
using CareSphere.Data.Organaizations.Impl;
using CareSphere.Data.Organaizations.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace CareSphere.Data.Configurations
{
    public static class DataLayerExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDbContext = null)
        {
            if (configureDbContext != null)
            {
                // ✅ Add CareSphereDbContext & OrderDbContext with provided configuration (SQL Server, InMemory, etc.)
                services.AddDbContext<CareSphereDbContext>(configureDbContext);
                services.AddDbContext<OrderDbContext>(configureDbContext);
            }
            else
            {
                // ✅ Ensure that DbContexts are registered, but do not override existing ones (Mock Support)
                services.TryAddScoped<CareSphereDbContext>();
                services.TryAddScoped<OrderDbContext>();
            }
            services.AddScoped<IUnitOfWork<CareSphereDbContext>, CareSphereUnitOfWork>();

            // ✅ Always register UnitOfWork and repositories (needed for DI)

            services.AddScoped<ICareSphereUnitOfWork, CareSphereUnitOfWork>();
            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
