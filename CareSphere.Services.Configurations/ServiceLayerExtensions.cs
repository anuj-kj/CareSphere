using CareSphere.Domains.Events;
using CareSphere.Services.Orders.Events.Handlers;
using CareSphere.Services.Orders.Impl;
using CareSphere.Services.Orders.Interfaces;
using CareSphere.Services.Organizations.Impl;
using CareSphere.Services.Organizations.Interfaces;
using CareSphere.Services.Users.Impl;
using CareSphere.Services.Users.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CareSphere.Services.Configurations
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
           // services.AddScoped<OrderEventHandlers>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
             services.AddSingleton<IDomainEventPublisher, DomainEventPublisher>();
          
          
            return services;
        }

    }
}
