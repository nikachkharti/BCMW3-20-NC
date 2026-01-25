using Forum.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Repository
{
    public interface ITopicRepository : IRepositoryBase<Topic, DbContext>
    {
    }
}
