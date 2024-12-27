namespace CareSphere.Data.Tests
{
    using CareSphere.Data.Configurations;
    using CareSphere.Data.Core.DataContexts;
    using CareSphere.Data.Core.Impl;
    using CareSphere.Data.Core.Interfaces;
    using CareSphere.Data.Organaizations.Impl;
    using CareSphere.Data.Organaizations.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using System.IO;

    public abstract class TestBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected IConfiguration Configuration { get; private set; }

        [SetUp]
        public void Setup()
        {
            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Configure services
            var services = new ServiceCollection();
            ConfigureServices(services);

            // Build service provider
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register application dependencies
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDataLayer(Configuration.GetConnectionString("DefaultConnection"));
        }
    }

}
