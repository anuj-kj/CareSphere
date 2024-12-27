using CareSphere.Services.Organizations.Impl;
using CareSphere.Services.Organizations.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CareSphere.Services.Configurations
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IOrganizationService, OrganizationService>();
            return services;
        }

    }
}
