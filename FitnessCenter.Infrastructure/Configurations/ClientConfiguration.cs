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
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ClientID");

            builder.Property(c => c.FirstName).IsRequired().HasMaxLength(25);
            builder.Property(c => c.LastName).IsRequired().HasMaxLength(25);
            builder.Property(c => c.MiddleName).HasMaxLength(25);
            builder.Property(c => c.PhoneNumber).HasMaxLength(25);
            builder.Property(c => c.Email).HasMaxLength(100);
            builder.Property(c => c.RegistrationDate).IsRequired();

            // Уникальные индексы (только когда значение не NULL)
            builder.HasIndex(c => c.PhoneNumber).IsUnique().HasFilter("[PhoneNumber] IS NOT NULL");
            builder.HasIndex(c => c.Email).IsUnique().HasFilter("[Email] IS NOT NULL");
            builder.HasIndex(c => c.LastName);

            builder.Ignore(x => x.CreatedAt);
            builder.Ignore(x => x.UpdatedAt);
        }
    }
}
