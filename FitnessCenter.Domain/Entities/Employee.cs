using FitnessCenter.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateOnly HireDate { get; set; } 
        public bool IsActive { get; set; } = true;
        public string? Photo { get; set; } = "Images/Employees/ImageByDefault.png";

        public User? User { get; set; }
    }
}
