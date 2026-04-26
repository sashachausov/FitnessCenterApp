using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Other
{
    public class EquipmentFilterDto
    {
        public string? SearchText { get; set; }
        public string? EquipmentType { get; set; }
        public string? Manufacturer { get; set; }
        public string? Status { get; set; }
    }
}
