using FitnessCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces.Repositories
{
    public interface IMembershipRepository
    {
        Task<Membership?> GetByIdAsync(int id);
        Task<IEnumerable<Membership>> GetAllByClientIdAsync(int clientId);
        Task<IEnumerable<Membership>> GetActiveAndFrozenByClientIdAsync(int clientId);
        Task<IEnumerable<Membership>> GetAllActiveAndFrozenWithClientsAsync();
        Task AddAsync(Membership membership);
        Task UpdateAsync(Membership membership);
        Task SaveChangesAsync();
    }
}
