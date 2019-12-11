using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RBTemplate.Domain.Entities.EventLog;
using RBTemplate.Infra.Data.Mappings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RBTemplate.Infra.Data.Contexts
{
    public class LogContext : DbContext
    {
        public DbSet<EventLog> EventLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventLogMap());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}
