using Forum.Application.Contracts.Repository;
using Forum.Domain.Entities;
using Forum.Infrastructure.Data;

namespace Forum.Infrastructure.Repository
{
    public class TopicRepository : RepositoryBase<Topic, ApplicationDbContext>, ITopicRepository
    {
        public TopicRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
