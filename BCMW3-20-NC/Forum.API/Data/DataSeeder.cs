using Forum.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data
{
    public static class DataSeeder
    {
        public static void SeedTopics(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>().HasData(
                new Topic
                {
                    Id = "9EC8A152-A054-4DE9-B30A-C9286E03ED72",
                    Title = "First Topic",
                    Content = "Hello World!",
                    CreateDate = new DateTime(2026, 01, 02, 0, 0, 0, DateTimeKind.Utc),
                    ImageUrl = null,
                    LastCommentDate = new DateTime(2026, 01, 02, 0, 0, 0, DateTimeKind.Utc),
                    CommentsAreAllowed = true,
                    AuthorId = "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E"
                },
                new Topic
                {
                    Id = "4B363C0A-6917-46FA-AAC0-815F2FD9351B",
                    Title = "Second Topic",
                    Content = "Hello World!",
                    CreateDate = new DateTime(2026, 01, 02, 0, 0, 0, DateTimeKind.Utc),
                    ImageUrl = null,
                    LastCommentDate = new DateTime(2026, 01, 02, 0, 0, 0, DateTimeKind.Utc),
                    CommentsAreAllowed = true,
                    AuthorId = "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F"
                },
                new Topic
                {
                    Id = "F9B01BD8-C9DB-4DA9-A1CD-46606EB0DADF",
                    Title = "Third Topic",
                    Content = "Hello World!",
                    CreateDate = new DateTime(2026, 01, 02, 0, 0, 0, DateTimeKind.Utc),
                    ImageUrl = null,
                    LastCommentDate = new DateTime(2026, 01, 02, 0, 0, 0, DateTimeKind.Utc),
                    CommentsAreAllowed = true,
                    AuthorId = "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F"
                }
            );
        }

        public static void SeedComments(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = "A59E8CEF-D838-4E7C-8396-86B1B3A96A98",
                    CommentDate = new DateTime(2026, 01, 02, 0, 0, 0, DateTimeKind.Utc),
                    Content = "Hello Comment!",
                    TopicId = "9EC8A152-A054-4DE9-B30A-C9286E03ED72",
                    AuthorId = "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F"
                },
                new Comment
                {
                    Id = "72775F5B-0878-483E-97EA-2BC255AA8B74",
                    CommentDate = new DateTime(2026, 01, 02, 0, 0, 0, DateTimeKind.Utc),
                    Content = "Hello Comment!",
                    TopicId = "4B363C0A-6917-46FA-AAC0-815F2FD9351B",
                    AuthorId = "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E"
                }
            );
        }

        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser()
                {
                    Id = "D2A3C6B1-5F4E-4F2A-9C3D-1E2F3A4B5C6D",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEJ3りんLw8+FvqR5z0t5K8xH/0GQbQZJ3dP2vN8kM1xQ7vJ3zX9rN2wK5fL8pY6mA==",
                    PhoneNumber = "555337681",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new ApplicationUser()
                {
                    Id = "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E",
                    UserName = "nika@gmail.com",
                    NormalizedUserName = "NIKA@GMAIL.COM",
                    Email = "nika@gmail.com",
                    NormalizedEmail = "NIKA@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEBvN8mK2xL9+TwpQ6r1u4T7Y/1FRcRaK4eQ3wO9lN2yR8wK6aM4{Y0sO3xM6gO7nB==",
                    PhoneNumber = "558490645",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new ApplicationUser()
                {
                    Id = "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F",
                    UserName = "gio@gmail.com",
                    NormalizedUserName = "GIO@GMAIL.COM",
                    Email = "gio@gmail.com",
                    NormalizedEmail = "GIO@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAECwP9nL3yM0+UxqR7s2v5U8Z/2GSdScL5fR4xP0mO3zS9xL7bN5aZ1tP4yN7hP8oC==",
                    PhoneNumber = "551442269",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            );
        }

        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "33B7ED72-9434-434A-82D4-3018B018CB87", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", Name = "Customer", NormalizedName = "CUSTOMER" }
            );
        }

        public static void SeedUserRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "33B7ED72-9434-434A-82D4-3018B018CB87",
                    UserId = "D2A3C6B1-5F4E-4F2A-9C3D-1E2F3A4B5C6D"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B",
                    UserId = "E3B4D7C2-6A5F-5A3B-0D4E-2F3A4B5C6D7E"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B",
                    UserId = "F4C5E8D3-7B6A-6B4C-1E5F-3A4B5C6D7E8F"
                }
            );
        }
    }
}
