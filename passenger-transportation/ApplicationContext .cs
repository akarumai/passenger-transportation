using Microsoft.EntityFrameworkCore;
using System;

namespace passenger_transportation
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Staff> Staff { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (baseDir.Contains("bin"))
            {
                int index = baseDir.IndexOf("bin");
                baseDir = baseDir.Substring(0, index);
            }
            optionsBuilder.UseSqlite($"Data Source={baseDir}transportation.db");
        }
    }
}