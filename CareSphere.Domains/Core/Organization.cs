using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareSphere.Domains.Core
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Staff> StaffMembers { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<PatientOrganization> PatientOrganizations { get; set; }
    }

}
