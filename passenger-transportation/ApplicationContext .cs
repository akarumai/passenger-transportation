using Microsoft.EntityFrameworkCore;

namespace passenger_transportation
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Staff> Staff { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=transportation.db");
        }
    }
}