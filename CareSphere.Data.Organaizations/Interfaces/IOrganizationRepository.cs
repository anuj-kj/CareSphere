using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.Interfaces;
using CareSphere.Domains.Core;


namespace CareSphere.Data.Organaizations.Interfaces
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
    }
}
