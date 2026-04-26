using FitnessCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("UserID");

            builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(128).HasColumnName("Password");
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(u => u.Employee).WithOne(e => e.User).HasForeignKey<Employee>(e => e.UserId).OnDelete(DeleteBehavior.SetNull);

            builder.Ignore(x => x.CreatedAt);
            builder.Ignore(x => x.UpdatedAt);
        }
    }
}
