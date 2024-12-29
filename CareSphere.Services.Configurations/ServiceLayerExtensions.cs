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
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

    }
}
