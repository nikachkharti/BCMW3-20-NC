using Forum.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔒 Static GUIDs
            var topic1Id = new Guid("9EC8A152-A054-4DE9-B30A-C9286E03ED72");
            var topic2Id = new Guid("4B363C0A-6917-46FA-AAC0-815F2FD9351B");
            var topic3Id = new Guid("F9B01BD8-C9DB-4DA9-A1CD-46606EB0DADF");

            var comment1Id = new Guid("A59E8CEF-D838-4E7C-8396-86B1B3A96A98");
            var comment2Id = new Guid("72775F5B-0878-483E-97EA-2BC255AA8B74");

            // 🔒 Static dates
            var createDate = new DateTime(2026, 1, 25);
            var lastCommentDate = new DateTime(2026, 1, 25);

            modelBuilder.Entity<Topic>().HasData(
                new Topic
                {
                    Id = topic1Id,
                    Title = "First Topic",
                    Content = "Hello World!",
                    CreateDate = createDate,
                    ImageUrl = null,
                    LastCommentDate = lastCommentDate,
                    CommentsAreAllowed = true
                },
                new Topic
                {
                    Id = topic2Id,
                    Title = "Second Topic",
                    Content = "Hello World!",
                    CreateDate = createDate,
                    ImageUrl = null,
                    LastCommentDate = lastCommentDate,
                    CommentsAreAllowed = true
                },
                new Topic
                {
                    Id = topic3Id,
                    Title = "Third Topic",
                    Content = "Hello World!",
                    CreateDate = createDate,
                    ImageUrl = null,
                    LastCommentDate = lastCommentDate,
                    CommentsAreAllowed = true
                }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = comment1Id,
                    CommentDate = new DateTime(2026, 1, 26),
                    Content = "Hello Comment!",
                    TopicId = topic1Id
                },
                new Comment
                {
                    Id = comment2Id,
                    CommentDate = new DateTime(2026, 1, 27),
                    Content = "Hello Comment!",
                    TopicId = topic2Id
                }
            );
        }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }

}
