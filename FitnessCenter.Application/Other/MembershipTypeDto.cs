using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Other
{
    public class MembershipTypeDto
    {
        public int TypeId { get; set; }
        public string MembershipName { get; set; } = string.Empty;
        public int? PeriodDays { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
