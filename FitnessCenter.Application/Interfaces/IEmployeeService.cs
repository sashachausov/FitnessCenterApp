using FitnessCenter.Application.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync();
        Task<List<EmployeeDto>> SearchAsync(EmployeeFilterDto filter);
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<List<RoleDto>> GetRolesAsync();
        Task<int> AddAsync(EmployeeDto entity, string? userName, string? password);
        Task<bool> UpdateAsync(EmployeeDto entity, string? userName, string? password, bool? resetPassword);
        Task<bool> DeleteAsync(int id);
    }
}
