using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Other
{
    public enum EmployeeSortOrder
    {
        Default,
        Alphabetical,
        HireDateAsc,
        HireDateDesc,
    }

    public class EmployeeFilterDto
    {
        public string? SearchText { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public DateOnly? HireDateFrom { get; set; }
        public DateOnly? HireDateTo { get; set; }
        public EmployeeSortOrder SortOrder { get; set; } = EmployeeSortOrder.Default;
    }
}
