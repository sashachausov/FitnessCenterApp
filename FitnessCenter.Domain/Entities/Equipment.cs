using FitnessCenter.Domain.Entities.Base;
using FitnessCenter.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Entities
{
    public class Equipment : BaseEntity
    {
        public int HallId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public string? InventoryNumber { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int Quantity { get; set; } = 1;
        public DateOnly? PurchaseDate { get; set; }
        public EquipmentStatus EquipmentStatus { get; set; } = EquipmentStatus.Good;
        public string? Photo { get; set; } = "Images/Equipment/ImageByDefault.png";

        public Hall Hall { get; set; } = null!;
    }
}
