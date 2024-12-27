using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareSphere.Domains.Core
{
    public class PatientOrganization
    {
        public int PatientOrganizationId { get; set; }
        public int PatientId { get; set; }
        public int OrganizationId { get; set; }

        // Navigation Properties
        public Patient Patient { get; set; }
        public Organization Organization { get; set; }
    }
}
