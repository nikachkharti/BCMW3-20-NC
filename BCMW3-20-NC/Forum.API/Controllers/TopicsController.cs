using Forum.API.Entities;
using Forum.API.Repository;
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
            var result = await _topicRepository.GetAllAsync();

            if (result.Items.Count == 0)
                return NotFound();

            return Ok(result.Items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleTopic(Guid id)
        {
            var topic = await _topicRepository.GetAsync(x => x.Id == id);

            if (topic == null)
                return NotFound();

            return Ok(topic);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTopic([FromBody] Topic topic)
        {
            await _topicRepository.AddAsync(topic);
            await _topicRepository.SaveAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromBody] Topic topic)
        {
            var topicToUpdate = await _topicRepository.GetAsync(x => x.Id == topic.Id);

            if (topicToUpdate == null)
                return NotFound();

            _topicRepository.Update(topic);
            await _topicRepository.SaveAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            var topicToDelete = await _topicRepository.GetAsync(x => x.Id == id);

            if (topicToDelete == null)
                return NotFound();

            _topicRepository.Remove(topicToDelete);
            await _topicRepository.SaveAsync();

            return NoContent();
        }
    }


}
