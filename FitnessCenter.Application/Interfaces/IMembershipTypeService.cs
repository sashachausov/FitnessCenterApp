using FitnessCenter.Application.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces
{
    public interface IMembershipTypeService
    {
        Task<List<MembershipTypeDto>> GetAllAsync();
        Task<List<MembershipTypeDto>> SearchAsync(string? nameFilter, decimal? maxPrice);
        Task<MembershipTypeDto?> GetByIdAsync(int id);
        Task<int> AddAsync(MembershipTypeDto dto);
        Task<bool> UpdateAsync(MembershipTypeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
