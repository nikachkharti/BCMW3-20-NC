using Forum.API.Data;
using Forum.API.Entities;

namespace Forum.API.Repository
{
    public class TopicRepository : RepositoryBase<Topic, ApplicationDbContext>, ITopicRepository
    {
        public TopicRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
