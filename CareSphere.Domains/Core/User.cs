using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CareSphere.Domains.Core
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public int? StaffId { get; set; }

        // Navigation Properties
     
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public Staff Staff { get; set; }
    }


}
