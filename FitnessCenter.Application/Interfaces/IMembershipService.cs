using FitnessCenter.Application.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces
{
    public interface IMembershipService
    {
        Task<IEnumerable<MembershipDto>> GetAllActiveAndFrozenMembershipsAsync();
        Task<IEnumerable<MembershipDto>> GetActiveAndFrozenByClientAsync(int clientId);
        Task<IEnumerable<MembershipDto>> GetMembershipHistoryAsync(int clientId);
        Task<MembershipDto?> GetByIdAsync(int membershipId);
        Task<MembershipDto> SellToNewClientAsync(string fullName, string phoneNumber, int typeId);
        Task<MembershipDto> SellToExistingClientAsync(int clientId, int typeId);
        Task<bool> ExtendMembershipAsync(int membershipId);
        Task<bool> SuspendMembershipAsync(int membershipId);
        Task<bool> CompleteMembershipAsync(int membershipId);
        Task<bool> CancelMembershipAsync(int membershipId);
    }
}
