using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;
using WebApplication1.Data.EF;

namespace WebApplication1.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<PriceTrip> PriceTrip { get; set; }
        public DbSet<Trip> Trips { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
              .HasIndex(e => e.phoneNumber)
              .IsUnique();
        }

    }
}