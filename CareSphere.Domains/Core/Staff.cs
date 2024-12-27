using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareSphere.Domains.Core
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TimeSpan? WorkingHoursStart { get; set; }
        public TimeSpan? WorkingHoursEnd { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int OrganizationId { get; set; }

        // Navigation Properties
        public Organization Organization { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }


}
