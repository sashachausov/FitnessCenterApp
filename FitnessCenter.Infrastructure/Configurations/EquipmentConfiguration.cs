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
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.ToTable("Equipment");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("EquipmentID");

            builder.Property(e => e.EquipmentName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.InventoryNumber).HasMaxLength(50);

            builder.Property(e => e.EquipmentType).HasConversion<int>().IsRequired();
            builder.Property(e => e.Manufacturer).HasConversion<int>().IsRequired();

            builder.Property(e => e.Quantity).IsRequired().HasDefaultValue(1);

            builder.Property(e => e.EquipmentStatus).HasConversion<int>().IsRequired().HasDefaultValue(EquipmentStatus.Good);
            builder.Property(e => e.Photo).HasMaxLength(500).HasDefaultValue("Images/Equipment/ImageByDefault.png");
            builder.HasIndex(e => e.InventoryNumber).IsUnique().HasFilter("[InventoryNumber] IS NOT NULL");
            builder.HasOne(e => e.Hall).WithMany(h => h.Equipment).HasForeignKey(e => e.HallId).OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(e => e.CreatedAt);
            builder.Ignore(e => e.UpdatedAt);
        }
    }
}
