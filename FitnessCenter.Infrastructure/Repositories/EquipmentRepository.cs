using FitnessCenter.Application.Interfaces.Repositories;
using FitnessCenter.Application.Other;
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
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly IDbContextFactory<FitnessCenterDbContext> _factory;

        public EquipmentRepository(IDbContextFactory<FitnessCenterDbContext> factory)
        {
            this._factory = factory;
        }

        public async Task<List<Equipment>> GetAllAsync()
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Equipment.Include(e => e.Hall).OrderBy(e => e.EquipmentName).AsNoTracking().ToListAsync();
        }

        public async Task<List<Equipment>> GetAllWithHallsAsync()
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Equipment.Include(e => e.Hall).OrderBy(e => e.Hall.HallName).ThenBy(e => e.EquipmentName).AsNoTracking().ToListAsync();
        }

        public async Task<List<Equipment>> SearchAsync(EquipmentFilterDto filter)
        {
            using var ctx = _factory.CreateDbContext();
            var query = ctx.Equipment.Include(e => e.Hall).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.EquipmentType))
            {
                var type = ParseEnumFromDescription<EquipmentType>(filter.EquipmentType);
                if (type.HasValue) query = query.Where(e => e.EquipmentType == type);
            }

            if (!string.IsNullOrWhiteSpace(filter.Manufacturer))
            {
                var manufacturer = ParseEnumFromDescription<Manufacturer>(filter.Manufacturer);
                if (manufacturer.HasValue) query = query.Where(e => e.Manufacturer == manufacturer);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                var status = ParseEnumFromDescription<EquipmentStatus>(filter.Status);
                if (status.HasValue) query = query.Where(e => e.EquipmentStatus == status);
            }

            var items = await query.OrderBy(e => e.Hall.HallName).ThenBy(e => e.EquipmentName).ToListAsync();

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.ToLower().Trim();
                items = items.Where(e =>
                    e.EquipmentName.ToLower().Contains(search) ||
                    (e.InventoryNumber != null && e.InventoryNumber.ToLower().Contains(search)) ||
                    GetEnumDescription(e.EquipmentType).ToLower().Contains(search) ||
                    GetEnumDescription(e.Manufacturer).ToLower().Contains(search) ||
                    GetEnumDescription(e.EquipmentStatus).ToLower().Contains(search)).ToList();
            }

            return items;
        }

        public async Task<Equipment?> GetByIdAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Equipment.Include(e => e.Hall).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Equipment equipment)
        {
            using var ctx = _factory.CreateDbContext();
            await ctx.Equipment.AddAsync(equipment);
            await ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Equipment equipment)
        {
            using var ctx = _factory.CreateDbContext();
            ctx.Equipment.Update(equipment);
            await ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            var equipment = await ctx.Equipment.FindAsync(id);
            if (equipment == null) return false;

            ctx.Equipment.Remove(equipment);
            await ctx.SaveChangesAsync();
            return true;
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        private static TEnum? ParseEnumFromDescription<TEnum>(string description) where TEnum : struct, Enum
        {
            foreach (TEnum value in Enum.GetValues<TEnum>())
            {
                if (GetEnumDescription(value) == description)
                    return value;
            }
            return null;
        }

        private static string GetEnumDescription<TEmum>(TEmum value) where TEmum : Enum
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                .FirstOrDefault() as System.ComponentModel.DescriptionAttribute;
            return attr?.Description ?? value.ToString();
        }
    }
}
