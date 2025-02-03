using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Configurations;
using CareSphere.Services.Configurations;
using CareSphere.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CareSphere.Services.Tests
{
    public abstract class TestBase : BaseTestSetup
    {
        protected TestBase() : base() { }

        [SetUp]
        public void Setup()
        {
            // Call the base setup method to ensure the base configuration is applied
            base.Setup().GetAwaiter().GetResult();

            // Additional setup for service tests can be done here if needed
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            // Register additional application dependencies
            services.AddServiceLayer();

            // Add other service dependencies here
        }
    }
}

