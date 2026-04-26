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
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbContextFactory<FitnessCenterDbContext> _factory;

        public RoleRepository(IDbContextFactory<FitnessCenterDbContext> factory)
        {
            _factory = factory;
        }
        public async Task<List<Role>> GetAllAsync()
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Roles.OrderBy(r => r.RoleName).AsNoTracking().ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Roles.FindAsync(id);
        }
    }
}
