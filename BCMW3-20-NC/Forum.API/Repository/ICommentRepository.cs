using Forum.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Repository
{
    public interface ICommentRepository : IRepositoryBase<Comment, DbContext>
    {
    }
}
