using EFCoreTutorial.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTutorial
{
    public class ApplicationDbContext : DbContext
    {
        private const string connectionString = @"Server=DESKTOP-SCSHELD\SQLEXPRESS;Database=EFTestDatabase;Trusted_Connection=True;TrustServerCertificate=True";

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
