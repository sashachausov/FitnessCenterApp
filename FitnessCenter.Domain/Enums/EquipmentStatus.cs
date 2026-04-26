using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Enums
{
    public enum EquipmentStatus
    {
        [System.ComponentModel.Description("Исправлено")]
        Good = 1,
        [System.ComponentModel.Description("Требует обслуживания")]
        NeedsMaintenance = 2,
        [System.ComponentModel.Description("На обслуживании")]
        UnderRepair = 3,
        [System.ComponentModel.Description("Списано")]
        Decommissioned = 4,
    }
}
