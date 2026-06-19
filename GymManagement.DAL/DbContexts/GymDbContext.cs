using GymManagement.DAL.Models;
using GymManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagement.DbContexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Plan> Plans { get; set; } 
        public DbSet<Booking> Bookings { get; set; }  
        public DbSet<Category> Categorys { get; set; }  
        public DbSet<HealthRecord> HealthRecords { get; set; }  
        public DbSet<Member> Members { get; set; }  
        public DbSet<MemperShip> MemperShips { get; set; }  
        public DbSet<Session> Sessions { get; set; }  
        public DbSet<Trainer> Trainers { get; set; }  
    }
} 
