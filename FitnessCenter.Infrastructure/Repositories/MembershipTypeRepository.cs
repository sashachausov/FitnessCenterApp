using FitnessCenter.Application.Interfaces.Repositories;
using FitnessCenter.Domain.Entities;
using FitnessCenter.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Infrastructure.Repositories
{
    public class MembershipTypeRepository : IMembershipTypeRepository
    {
        private readonly IDbContextFactory<FitnessCenterDbContext> _factory;

        public MembershipTypeRepository(IDbContextFactory<FitnessCenterDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<MembershipType>> GetAllAsync()
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.MembershipTypes.OrderBy(mt => mt.Id).AsNoTracking().ToListAsync();
        }

        public async Task<MembershipType?> GetByIdAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.MembershipTypes.FindAsync(id);
        }

        public async Task<List<MembershipType>> SearchAsync(string? nameFilter, decimal? maxPrice)
        {
            using var ctx = _factory.CreateDbContext();
            var query = ctx.MembershipTypes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                query = query.Where(mt => mt.MembershipName.Contains(nameFilter));
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(mt => mt.Price <= maxPrice.Value);
            }

            return await query.OrderBy(mt => mt.Id).ToListAsync();
        }

        public async Task AddAsync(MembershipType type)
        {
            using var ctx = _factory.CreateDbContext();
            await ctx.MembershipTypes.AddAsync(type);
            await ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(MembershipType type)
        {
            using var ctx = _factory.CreateDbContext();
            ctx.MembershipTypes.Update(type);
            await ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            var type = await ctx.MembershipTypes.FindAsync(id);
            if (type == null) return false;
            ctx.MembershipTypes.Remove(type);
            await ctx.SaveChangesAsync();
            return true;
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
