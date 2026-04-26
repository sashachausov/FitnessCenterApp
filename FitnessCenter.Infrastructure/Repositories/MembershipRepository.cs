using FitnessCenter.Application.Interfaces.Repositories;
using FitnessCenter.Domain.Entities;
using FitnessCenter.Domain.Enums;
using FitnessCenter.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Infrastructure.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly IDbContextFactory<FitnessCenterDbContext> _factory;

        public MembershipRepository(IDbContextFactory<FitnessCenterDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<Membership?> GetByIdAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Memberships
                .Include(m => m.Type)
                .Include(m => m.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Membership>> GetAllByClientIdAsync(int clientId)
        {
            using var ctx = _factory.CreateDbContext();
            // Include Client so DTO mapping that reads ClientFullName has the data
            return await ctx.Memberships
                .Include(m => m.Type)
                .Include(m => m.Client)
                .Where(m => m.ClientId == clientId)
                .OrderByDescending(m => m.SellDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Membership>> GetActiveAndFrozenByClientIdAsync(int clientId)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Memberships
                .Include(m => m.Type)
                .Include(m => m.Client)
                .Where(m => m.ClientId == clientId &&
                            (m.Status == MembershipStatus.Active || m.Status == MembershipStatus.Frozen))
                .OrderByDescending(m => m.SellDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Membership>> GetAllActiveAndFrozenWithClientsAsync()
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Memberships
                .Include(m => m.Type)
                .Include(m => m.Client)
                .Where(m => m.Status == MembershipStatus.Active || m.Status == MembershipStatus.Frozen)
                .OrderBy(m => m.Id)
                .ToListAsync();
        }

        public async Task AddAsync(Membership membership)
        {
            using var ctx = _factory.CreateDbContext();
            await ctx.Memberships.AddAsync(membership);
            await ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Membership membership)
        {
            using var ctx = _factory.CreateDbContext();
            ctx.Memberships.Update(membership);
            await ctx.SaveChangesAsync();
        }

        // SaveChangesAsync is a no-op because Add/Update persist immediately in this implementation.
        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
