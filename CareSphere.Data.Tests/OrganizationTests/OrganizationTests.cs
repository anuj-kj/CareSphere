
using CareSphere.Data.Core.DataContexts;
using CareSphere.Data.Core.Interfaces;
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
            var unitOfWork = ServiceProvider.GetRequiredService<ICareSphereUnitOfWork>();  // ✅ Correct

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
