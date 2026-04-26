using FitnessCenter.Domain.Enums;
using FitnessCenter.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Entities
{
    public class Membership : BaseEntity
    {
        public int ClientId { get; set; }
        public int TypeId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal ActualCost { get; set; }
        public DateOnly SellDate { get; set; }

        public MembershipStatus Status { get; set; } = MembershipStatus.Active;
        public DateOnly? FrozenDate { get; set; }
        public int? FreezeDaysUsed { get; set; }
        public bool HasBeenFrozen { get; set; }

        public Client Client { get; set; } = null!;
        public MembershipType Type { get; set; } = null!;
    }
}
