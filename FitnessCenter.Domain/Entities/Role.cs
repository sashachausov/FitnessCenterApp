using FitnessCenter.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool HasFullAccess { get; set; } = false;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
