using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.Impl;
using CareSphere.Data.Core.Interfaces;
using CareSphere.Data.Organaizations.Impl;
using CareSphere.Data.Organaizations.Interfaces;
using CareSphere.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CareSphere.Data.Tests.OrganizationTests
{
    [TestFixture]
    public class OrganizationTests : BaseTestSetup
    {
        private IOrganizationRepository _organizationRepository;
        [SetUp]
        public void Initialize()
        {
            var unitOfWork= ServiceProvider.GetRequiredService<IUnitOfWork>();
           // _organizationRepository = unitOfWork.GetRepository<IOrganizationRepository>();
            _organizationRepository = ServiceProvider.GetRequiredService<IOrganizationRepository>();
        }
        [Test]
        public async Task GeALL_Should_GetAllSuccessfully()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            
        }
    }
}
