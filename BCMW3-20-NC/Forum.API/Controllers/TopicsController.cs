using Forum.API.Entities;
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

            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleTopic(Guid id)
        {
            var topic = await _topicRepository.GetSingleTopicAsync(id);

            if (topic == null)
                return NotFound();

            return Ok(topic);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTopic([FromBody] Topic topic)
        {
            await _topicRepository.AddNewTopicAsync(topic);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromBody] Topic topic)
        {
            await _topicRepository.UpdateNewTopicAsync(topic);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            var deleted = await _topicRepository.DeleteSingleTopicAsync(id);

            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }
    }

}
