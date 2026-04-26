using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Other
{
    public class EquipmentDto
    {
        public int EquipmentId { get; set; }
        public int HallId { get; set; }
        public string HallName { get; set; } = string.Empty;
        public string EquipmentName { get; set; } = string.Empty;
        public string? InventoryNumber { get; set; }
        public string EquipmentType { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public string EquipmentStatus { get; set; } = string.Empty;
        public string? PhotoPath { get; set; }

        public string DisplayName => $"{EquipmentType} {EquipmentName} ({Manufacturer})".Trim();
    }
}
