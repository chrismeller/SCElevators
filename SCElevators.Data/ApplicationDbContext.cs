using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using SCElevators.Data.Models;

namespace SCElevators.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(string connectionString) : base(BuildOptions(connectionString))
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        private static DbContextOptions<ApplicationDbContext> BuildOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Elevator>()
                .HasIndex(e => e.Number)
                .IsUnique();

            modelBuilder.Entity<Elevator>()
                .HasIndex(e => e.County);
        }

        public DbConnection GetDbConnection()
        {
            return Database.GetDbConnection();
        }

        public DbSet<Elevator> Elevators { get; set; }
    }
}