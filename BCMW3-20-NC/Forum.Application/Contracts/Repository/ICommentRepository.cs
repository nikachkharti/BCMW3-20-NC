using Forum.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.Contracts.Repository
{
    public interface ICommentRepository : IRepositoryBase<Comment, DbContext>
    {
    }
}
