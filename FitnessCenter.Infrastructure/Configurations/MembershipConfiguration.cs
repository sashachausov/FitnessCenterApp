using FitnessCenter.Domain.Entities;
using FitnessCenter.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Infrastructure.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.ToTable("Memberships");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).HasColumnName("MembershipID");

            builder.Property(m => m.StartDate).IsRequired();
            builder.Property(m => m.EndDate);
            builder.Property(m => m.ActualCost).IsRequired().HasPrecision(12, 2);
            builder.Property(m => m.SellDate).IsRequired();

            // Новое: статус как int с конвертацией
            builder.Property(m => m.Status).HasConversion<int>().IsRequired().HasDefaultValue(MembershipStatus.Active);

            builder.Property(m => m.FrozenDate);
            builder.Property(m => m.FreezeDaysUsed);
            builder.Property(m => m.HasBeenFrozen).IsRequired().HasDefaultValue(false);

            // Индексы
            builder.HasIndex(m => m.ClientId);
            builder.HasIndex(m => m.EndDate);
            builder.HasIndex(m => m.Status);

            // Связи
            builder.HasOne(m => m.Client).WithMany(c => c.Memberships).HasForeignKey(m => m.ClientId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Type).WithMany(mt => mt.Memberships).HasForeignKey(m => m.TypeId).OnDelete(DeleteBehavior.Restrict);

            // Игнорируем поля аудита, т.к. используем кастомные даты
            builder.Ignore(m => m.CreatedAt);
            builder.Ignore(m => m.UpdatedAt);
        }
    }
}
