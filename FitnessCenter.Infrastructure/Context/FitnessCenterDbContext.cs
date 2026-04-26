using FitnessCenter.Domain.Entities;
using FitnessCenter.Domain.Entities.Base;
using FitnessCenter.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Infrastructure.Context
{
    public class FitnessCenterDbContext : DbContext
    {
        public FitnessCenterDbContext(DbContextOptions<FitnessCenterDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<MembershipType> MembershipTypes { get; set; } = null!;
        public DbSet<Membership> Memberships { get; set; } = null!;
        public DbSet<Hall> Halls { get; set; } = null!;
        public DbSet<Equipment> Equipment { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitnessCenterDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var createdAtProperty = entityType.FindProperty(nameof(BaseEntity.CreatedAt));
                if (createdAtProperty != null)
                {
                    createdAtProperty.SetDefaultValueSql("GETUTCDATE()");
                    createdAtProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
