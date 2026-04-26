using FitnessCenter.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Entities
{
    public class MembershipType : BaseEntity
    {
        public string MembershipName { get; set; } = string.Empty;
        public int? PeriodDays { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    }
}
