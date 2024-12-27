using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.DataContexts;
using CareSphere.Data.Core.Impl;
using CareSphere.Data.Organaizations.Interfaces;
using CareSphere.Domains.Core;
using Microsoft.EntityFrameworkCore;

namespace CareSphere.Data.Organaizations.Impl
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(CareSphereDbContext context) : base(context) { }
    }
}
