using Forum.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }

}
