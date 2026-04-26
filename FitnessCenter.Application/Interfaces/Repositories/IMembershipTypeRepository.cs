using FitnessCenter.Application.Other;
using FitnessCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces.Repositories
{
    public interface IMembershipTypeRepository
    {
        Task<List<MembershipType>> GetAllAsync();
        Task<MembershipType?> GetByIdAsync(int id);
        Task<List<MembershipType>> SearchAsync(string? nameFilter, decimal? maxPrice);
        Task AddAsync(MembershipType type);
        Task UpdateAsync(MembershipType type);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
