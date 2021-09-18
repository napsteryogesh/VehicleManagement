using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ScalableVehicleService.Model;
using Microsoft.EntityFrameworkCore.Design;

namespace ScalableVehicleService.DAL
{
    public class VehicleDbContextFactory : IDesignTimeDbContextFactory<VehicleDbContext>
    {
        private readonly string _connectionString = @"Data Source=C:\Demo\VehicleStoreDb.db;";

        public VehicleDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VehicleDbContext>();

            return new VehicleDbContext(optionsBuilder.Options);
        }
    }

    public class VehicleDbContext : DbContext
    {
        private readonly string _connectionString = @"Data Source=C:\Demo\VehicleStoreDb.db;";

        private static bool _created = false;
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options)
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureCreated();
            }
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
            modelBuilder.Entity<Vehicle>().HasKey(obj => obj.VehicleId);
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Location>().HasKey(obj => obj.Id);
        }

    }
}
