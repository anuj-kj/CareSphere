using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.DataContexts;
using CareSphere.Data.Core.Interfaces;
using CareSphere.Data.Organaizations.Interfaces;
using CareSphere.Domains.Core;
using CareSphere.Services.Organizations.Interfaces;

namespace CareSphere.Services.Organizations.Impl
{
    public class OrganizationService : IOrganizationService
    {
        private ICareSphereUnitOfWork UnitOfWork { get; set; }
        private IOrganizationRepository OrganizationRepository { get; set; }
        public OrganizationService(ICareSphereUnitOfWork unitOfWork, IOrganizationRepository organizationRepository)
        {
            UnitOfWork = unitOfWork;
            OrganizationRepository = organizationRepository;
        }

        public async Task<List<Organization>> GetOrganizations()
        {
            var organizations= await OrganizationRepository.GetAllAsync();
            return organizations.ToList();
        }

    }
}
