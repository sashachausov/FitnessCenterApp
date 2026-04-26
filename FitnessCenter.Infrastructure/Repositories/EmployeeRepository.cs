using FitnessCenter.Application.Interfaces.Repositories;
using FitnessCenter.Application.Other;
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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbContextFactory<FitnessCenterDbContext> _factory;

        public EmployeeRepository(IDbContextFactory<FitnessCenterDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Employees
                .Include(e => e.User)
                .ThenInclude(u => u.Role)
                .OrderBy(e => e.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Employee>> SearchAsync(EmployeeFilterDto filter)
        {
            using var ctx = _factory.CreateDbContext();
            var query = ctx.Employees
                .Include(e => e.User)
                .ThenInclude(u => u.Role)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(e => e.LastName.Contains(filter.SearchText) ||
                                         e.FirstName.Contains(filter.SearchText) ||
                                         (e.MiddleName != null && e.MiddleName.Contains(filter.SearchText)) ||
                                         (e.PhoneNumber != null && e.PhoneNumber.Contains(filter.SearchText)) ||
                                         (e.Email != null && e.Email.Contains(filter.SearchText)));
            }

            if (filter.RoleId.HasValue)
                query = query.Where(e => e.User != null && e.User.RoleId == filter.RoleId.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(e => e.IsActive == filter.IsActive.Value);

            if (filter.HireDateFrom.HasValue)
                query = query.Where(e => e.HireDate >= filter.HireDateFrom.Value);

            if (filter.HireDateTo.HasValue)
                query = query.Where(e => e.HireDate <= filter.HireDateTo.Value);

            query = filter.SortOrder switch
            {
                EmployeeSortOrder.Alphabetical => query.OrderBy(e => e.LastName).ThenBy(e => e.FirstName),
                EmployeeSortOrder.HireDateAsc => query.OrderBy(e => e.HireDate),
                EmployeeSortOrder.HireDateDesc => query.OrderByDescending(e => e.HireDate),
                _ => query.OrderBy(e => e.Id)
            };

            return await query.ToListAsync();
        }

        public async Task<Employee?> GetByIdWithUserAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            return await ctx.Employees
                .Include(e => e.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Employee employee)
        {
            using var ctx = _factory.CreateDbContext();
            await ctx.Employees.AddAsync(employee);
            await ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            using var ctx = _factory.CreateDbContext();
            ctx.Employees.Update(employee);
            await ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var ctx = _factory.CreateDbContext();
            var employee = await ctx.Employees.FindAsync(id);
            if (employee == null) return false;

            ctx.Employees.Remove(employee);
            await ctx.SaveChangesAsync();
            return true;
        }
    }
}
