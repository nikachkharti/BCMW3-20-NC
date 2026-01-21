using Forum.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicRepository _topicRepository;

        public TopicsController(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var result = await _topicRepository.GetAllTopicsAsync();

            if (result.Count > 0)
                return Ok(result);
            return NotFound();
        }
    }
}
