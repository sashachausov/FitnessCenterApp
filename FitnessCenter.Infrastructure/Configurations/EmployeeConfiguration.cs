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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("EmployeeID");

            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(25);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(25);
            builder.Property(e => e.MiddleName).HasMaxLength(25);
            builder.Property(e => e.PhoneNumber).HasMaxLength(25);
            builder.Property(e => e.Email).HasMaxLength(100);
            builder.Property(e => e.HireDate).IsRequired();
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.Photo).HasMaxLength(500).HasDefaultValue("Images/Employees/ImageByDefault.png");
            
            // Уникальный индекс по Email (только когда не NULL)
            builder.HasIndex(e => e.Email).IsUnique().HasFilter("[Email] IS NOT NULL");

            // Индексы для поиска
            builder.HasIndex(e => e.PhoneNumber);
            builder.HasIndex(e => e.LastName);

            // Связь с User (один сотрудник - один пользователь, но UserId может быть NULL)
            builder.HasOne(e => e.User).WithOne(u => u.Employee).HasForeignKey<Employee>(e => e.UserId).OnDelete(DeleteBehavior.SetNull);

            builder.Ignore(e => e.CreatedAt);
            builder.Ignore(e => e.UpdatedAt);
        }
    }
}
