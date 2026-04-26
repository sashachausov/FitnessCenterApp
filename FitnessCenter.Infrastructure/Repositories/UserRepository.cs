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
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<FitnessCenterDbContext> _factory;

        public UserRepository(IDbContextFactory<FitnessCenterDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<User?> GetByUserNameWithRoleAsync(string userName)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task AddAsync(User user)
        {
            using var ctx = _factory.CreateDbContext();
            await ctx.Users.AddAsync(user);
            await ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            using var ctx = _factory.CreateDbContext();
            ctx.Users.Update(user);
            await ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            var user = await ctx.Users.FindAsync(id);
            if (user == null) return false;

            ctx.Users.Remove(user);
            await ctx.SaveChangesAsync();
            return true;
        }
    }
}
