using FitnessCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("RoleID");

            builder.Property(r => r.RoleName).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Description).HasMaxLength(500);
            builder.Property(r => r.HasFullAccess).IsRequired().HasDefaultValue(false);
            builder.HasIndex(r => r.RoleName).IsUnique();

            builder.Ignore(x => x.CreatedAt);
            builder.Ignore(x => x.UpdatedAt);
        }
    }
}
