using FitnessCenter.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Entities
{
    public class Hall : BaseEntity
    {
        public string HallName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Capacity { get; set; } = 0;

        public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
    }
}
