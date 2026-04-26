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
    public class ClientRepository : IClientRepository
    {
        private readonly IDbContextFactory<FitnessCenterDbContext> _factory;

        public ClientRepository(IDbContextFactory<FitnessCenterDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Client>> GetAllAsync()
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Clients.OrderBy(c => c.Id).AsNoTracking().ToListAsync();
        }

        public async Task<List<Client>> SearchAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetAllAsync();

            searchText = searchText.ToLower().Trim();

            using var ctx = _factory.CreateDbContext();
            return await ctx.Clients
                .Where(c =>
                    c.FirstName.ToLower().Contains(searchText) ||
                    c.LastName.ToLower().Contains(searchText) ||
                    (c.MiddleName != null && c.MiddleName.ToLower().Contains(searchText)) ||
                    (c.PhoneNumber != null && c.PhoneNumber.Contains(searchText)) ||
                    (c.Email != null && c.Email.ToLower().Contains(searchText)))
                .OrderBy(c => c.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Clients.FindAsync(id);
        }

        public async Task AddAsync(Client client)
        {
            using var ctx = _factory.CreateDbContext();
            await ctx.Clients.AddAsync(client);
            await ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            using var ctx = _factory.CreateDbContext();
            ctx.Clients.Update(client);
            await ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            var client = await ctx.Clients.FindAsync(id);
            if (client == null) return false;

            bool hasActiveMembership = await ctx.Memberships.AnyAsync(m => m.ClientId == id && (m.Status == MembershipStatus.Active || m.Status == MembershipStatus.Frozen));
            if (hasActiveMembership)
                throw new InvalidOperationException("Невозможно удалить клиента: у клиента есть действующий или приостановленный абонемент. Завершите или отмените абонемент перед удалением.");

            ctx.Clients.Remove(client);
            await ctx.SaveChangesAsync();
            return true;
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
