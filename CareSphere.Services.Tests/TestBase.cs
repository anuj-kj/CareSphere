using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Configurations;
using CareSphere.Services.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CareSphere.Services.Tests
{
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

            var databaseProvider = Configuration["DatabaseProvider"];
            if (databaseProvider == "SQLite")
            {
                services.AddDataLayer(options => options.UseSqlite(Configuration.GetConnectionString("SQLiteConnection")));
            }
            else
            {
                services.AddDataLayer(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
        }
    }
}
