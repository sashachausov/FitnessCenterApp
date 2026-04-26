using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Other
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public int? UserId { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateOnly? HireDate { get; set; }
        public bool IsActive { get; set; }
        public string? PhotoPath { get; set; }

        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? UserName { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}
