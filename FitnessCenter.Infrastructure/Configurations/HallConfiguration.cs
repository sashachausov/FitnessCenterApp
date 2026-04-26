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
    public class HallConfiguration : IEntityTypeConfiguration<Hall>
    {
        public void Configure(EntityTypeBuilder<Hall> builder)
        {
            builder.ToTable("Halls");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("HallID");

            builder.Property(x => x.HallName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Capacity).IsRequired().HasDefaultValue(0);
            builder.HasIndex(x => x.HallName).IsUnique();
            builder.HasMany(x => x.Equipment).WithOne(e => e.Hall).HasForeignKey(e => e.HallId).OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(x => x.CreatedAt);
            builder.Ignore(x => x.UpdatedAt);
        }
    }
}
