using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Domain.Enums
{
    public enum MembershipStatus
    {
        [System.ComponentModel.Description("Действует")]
        Active = 1,
        [System.ComponentModel.Description("Приостановлен")]
        Frozen = 2,
        [System.ComponentModel.Description("Завершен")]
        Completed = 3,
        [System.ComponentModel.Description("Отменён")]
        Canceled = 4
    }
}
