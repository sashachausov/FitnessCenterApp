using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Enums
{
    public enum Manufacturer
    {
        [System.ComponentModel.Description("Technogym")]
        Technogym = 1,
        [System.ComponentModel.Description("Life Fitness")]
        LifeFitness = 2,
        [System.ComponentModel.Description("Precor")]
        Precor = 3,
        [System.ComponentModel.Description("Matrix")]
        Matrix = 4,
        [System.ComponentModel.Description("Hammer Strenght")]
        HammerStrength = 5,
        [System.ComponentModel.Description("Eleiko")]
        Eleiko = 6,
        [System.ComponentModel.Description("Другое")]
        Other = 99,
    }
}
