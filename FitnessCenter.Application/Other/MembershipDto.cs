using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Other
{
    public class MembershipDto
    {
        public int MembershipId { get; set; }
        public int ClientId { get; set; }
        public string ClientFullName { get; set; } = string.Empty;
        public int TypeId { get; set; }
        public string MembershipName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal ActualCost { get; set; }
        public DateOnly SellDate { get; set; }
        public string MembershipStatus { get; set; } = string.Empty;
        public string? StatusDisplayName { get; set; }

        // Вычисляемое свойство для отображения периода
        public string PeriodDisplay => EndDate.HasValue ? $"{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}" : $"{StartDate:dd.MM.yyyy} - бессрочно";
    }
}
