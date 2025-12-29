using EFCoreTableRelationsTutorial.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;
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
        public DbSet<StudentCourses> StudentCourses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<UserProfile>().HasKey(p => p.Id);
            modelBuilder.Entity<Author>().HasKey(a => a.Id);
            modelBuilder.Entity<Book>().HasKey(b => b.Id);
            modelBuilder.Entity<Student>().HasKey(s => s.Id);
            modelBuilder.Entity<Course>().HasKey(c => c.Id);

            modelBuilder
                .Entity<User>().Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourses",
                    j =>
                    {
                        j.HasKey("StudentId", "CourseId"); // 👈 Composite PK
                        j.ToTable("StudentCourses");
                    });



            SeedUsers(modelBuilder);
            SeedUserProfiles(modelBuilder);
            SeedAuthors(modelBuilder);
            SeedBooks(modelBuilder);
            SeedStudents(modelBuilder);
            SeedCourses(modelBuilder);



            modelBuilder
                .Entity("StudentCourses")
                .HasData
                (
                    new { StudentId = 1, CourseId = 1 },
                    new { StudentId = 1, CourseId = 2 },
                    new { StudentId = 2, CourseId = 2 }
                );

        }


        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Name = "John Doe"
                },
                new User()
                {
                    Id = 2,
                    Name = "Jack Doe"
                }
            );
        }
        private static void SeedUserProfiles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile()
                {
                    Id = 1,
                    Address = "Rustavelis 1",
                    UserId = 1
                },
                new UserProfile()
                {
                    Id = 2,
                    Address = "Rustavelis 23",
                    UserId = 2
                }
            );
        }
        private static void SeedAuthors(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData
            (
                new Author()
                {
                    Id = 1,
                    FullName = "Ilia Chavchavadze"
                }
            );
        }
        private static void SeedBooks(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData
            (
                new Book()
                {
                    Id = 1,
                    Title = "მგზავრის წერილები",
                    AuthorId = 1
                },
                new Book()
                {
                    Id = 2,
                    Title = "კაცია ადამიანი",
                    AuthorId = 1
                }
            );
        }
        private static void SeedStudents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData
            (
                new Student()
                {
                    Id = 1,
                    Name = "Nika"
                },
                new Student()
                {
                    Id = 2,
                    Name = "Giorgi"
                }
            );
        }
        private static void SeedCourses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData
            (
                new Course()
                {
                    Id = 1,
                    Title = "C# For Beginners"
                },
                new Course()
                {
                    Id = 2,
                    Title = "Frontend"
                }
            );
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
