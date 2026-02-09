using Forum.Application.Contracts.Repository;
using Forum.Domain.Entities;
using Forum.Infrastructure.Data;

namespace Forum.Infrastructure.Repository
{
    public class CommentRepository : RepositoryBase<Comment, ApplicationDbContext>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
