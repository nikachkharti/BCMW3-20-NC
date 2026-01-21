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

            Guid topic1Id = Guid.NewGuid();
            Guid topic2Id = Guid.NewGuid();
            Guid topic3Id = Guid.NewGuid();

            Guid comment1Id = Guid.NewGuid();
            Guid comment2Id = Guid.NewGuid();

            modelBuilder.Entity<Topic>().HasData
            (
                new Topic()
                {
                    Id = topic1Id,
                    Title = "First Topic",
                    Content = "Hello World!",
                    CreateDate = DateTime.Now,
                    ImageUrl = null,
                    LastCommentDate = DateTime.Now,
                    CommentsAreAllowed = true
                },
                new Topic()
                {
                    Id = topic2Id,
                    Title = "Second Topic",
                    Content = "Hello World!",
                    CreateDate = DateTime.Now,
                    ImageUrl = null,
                    LastCommentDate = DateTime.Now,
                    CommentsAreAllowed = true
                },
                new Topic()
                {
                    Id = topic3Id,
                    Title = "Second Topic",
                    Content = "Hello World!",
                    CreateDate = DateTime.Now,
                    ImageUrl = null,
                    LastCommentDate = DateTime.Now,
                    CommentsAreAllowed = true
                }
            );

            modelBuilder.Entity<Comment>().HasData
            (
                new Comment()
                {
                    Id = comment1Id,
                    CommentDate = DateTime.Now.AddDays(1),
                    Content = "Hello Comment!",
                    TopicId = topic1Id,
                },
                new Comment()
                {
                    Id = comment2Id,
                    CommentDate = DateTime.Now.AddDays(2),
                    Content = "Hello Comment!",
                    TopicId = topic2Id,
                }
            );

        }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }

}
