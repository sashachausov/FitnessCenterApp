using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Other
{
    public class MembershipNewDto
    {
        public int ClientId { get; set; }
        public int MembershipTypeId { get; set; }
        public decimal ActualCost { get; set; }
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public string? NewClientFullName { get; set; }
        public string? NewClientPhoneNumber { get; set; }

        public bool IsNewClient => !string.IsNullOrWhiteSpace(NewClientFullName);
    }
}
