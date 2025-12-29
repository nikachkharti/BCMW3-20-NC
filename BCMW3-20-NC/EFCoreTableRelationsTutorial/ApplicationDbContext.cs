using EFCoreTableRelationsTutorial.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace EFCoreTableRelationsTutorial
{
    public class ApplicationDbContext : DbContext
    {
        private const string connectionString = @"Server=DESKTOP-SCSHELD\SQLEXPRESS;Database=EFTestDatabaseRelations;Trusted_Connection=True;TrustServerCertificate=True";


        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
