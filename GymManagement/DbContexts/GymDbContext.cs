using GymManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagement.DbContexts
{
    public class GymDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=GymDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<Plan> Plans { get; set; } 
    }
}
