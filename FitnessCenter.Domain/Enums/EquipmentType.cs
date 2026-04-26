using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Enums
{
    public enum EquipmentType
    {
        [System.ComponentModel.Description("Кардиотренажёр")]
        Cardio = 1,
        [System.ComponentModel.Description("Силовой тренажёр")]
        Strength = 2,
        [System.ComponentModel.Description("Свободные веса")]
        FreeWeights = 3,
        [System.ComponentModel.Description("Функциональный тренинг")]
        Functional = 4,
        [System.ComponentModel.Description("Аксессуары")]
        Accessories = 5,
    }
}
