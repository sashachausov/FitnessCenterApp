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
    public class MembershipTypeConfiguration : IEntityTypeConfiguration<MembershipType>
    {
        public void Configure(EntityTypeBuilder<MembershipType> builder)
        {
            builder.ToTable("MembershipTypes");
            builder.HasKey(mt => mt.Id);
            builder.Property(mt => mt.Id).HasColumnName("TypeID");

            builder.Property(mt => mt.MembershipName).IsRequired().HasMaxLength(50);
            builder.Property(mt => mt.Description).HasMaxLength(500);
            builder.Property(mt => mt.Price).IsRequired().HasPrecision(12, 2);
            builder.Property(mt => mt.PeriodDays).HasDefaultValue(null);

            builder.Ignore(mt => mt.CreatedAt);
            builder.Ignore(mt => mt.UpdatedAt);
        }
    }
}
