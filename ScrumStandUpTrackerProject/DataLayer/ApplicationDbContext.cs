using Microsoft.EntityFrameworkCore;
using ScrumStandUpTrackerProject.Models;

namespace ScrumStandUpTrackerProject.DataLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<DailyStatus> DailyStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Developer>()
                .HasIndex(d => d.Email)
                .IsUnique();

        }

    }
}