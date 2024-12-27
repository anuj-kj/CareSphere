using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.Interfaces;
using CareSphere.Data.Organaizations.Impl;
using CareSphere.Data.Organaizations.Interfaces;
using CareSphere.Services.Organizations.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CareSphere.Services.Tests.Organizations
{
    public class OrganizationServiceTests:TestBase
    {
        private IOrganizationService _organizationService;
        [SetUp]
        public void Initialize()
        {
            _organizationService = ServiceProvider.GetRequiredService<IOrganizationService>();
        }
        [Test]
        public async Task GeALL_Should_GetAllSuccessfully()
        {
            var organizations = await _organizationService.GetOrganizations();
            Assert.That(organizations.Count, Is.GreaterThan(0));

        }
    }
}
