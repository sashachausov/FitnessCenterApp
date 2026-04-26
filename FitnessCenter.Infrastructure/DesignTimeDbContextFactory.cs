using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using FitnessCenter.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FitnessCenterDbContext>
    {
        public FitnessCenterDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "FitnessCenter.UI"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection String 'DefaultConnection' not found in appsettings.json");

            var optionsBuilder = new DbContextOptionsBuilder<FitnessCenterDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FitnessCenterDbContext(optionsBuilder.Options);
        }
    }
}
