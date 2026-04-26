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
    public class HallRepository : IHallRepository
    {
        private readonly IDbContextFactory<FitnessCenterDbContext> _factory;

        public HallRepository(IDbContextFactory<FitnessCenterDbContext> factory)
        {
            this._factory = factory;            
        }

        public async Task<List<Hall>> GetAllAsync()
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Halls.OrderBy(h => h.Id).AsNoTracking().ToListAsync();
        }

        public async Task<Hall?> GetByIdAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Halls.FindAsync(id);
        }
    }
}
