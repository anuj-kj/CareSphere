using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Domains.Core;

namespace CareSphere.Services.Organizations.Interfaces
{
    public interface IOrganizationService
    {
        Task<List<Organization>> GetOrganizations();
    }
}
