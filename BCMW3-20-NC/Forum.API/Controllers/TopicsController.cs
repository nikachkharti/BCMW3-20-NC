using Forum.API.Models.DTO.Topics;
using Forum.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicsController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopics([FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        {
            var result = await _topicService.GetAllTopicsAsync(
                pageNumber: pageNumber,
                pageSize: pageSize
            );

            return Ok(new
            {
                Items = result.Topics,
                TotalCount = result.TotalCount
            });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleTopic(Guid id)
        {
            var result = await _topicService.GetTopicDetailsAsync(id);

            if (result != null)
                return Ok(result);

            return NotFound($"Topic with id: {id} not found");

        }

        [HttpPost]
        public async Task<IActionResult> AddNewTopic([FromBody] TopicForCreatingDto model)
        {
            var result = await _topicService.AddNewTopicAsync(model);

            if (result != 0)
                return Created();

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromBody] TopicForUpdatingDto topic)
        {
            var result = await _topicService.UpdateNewTopicAsync(topic);

            if (result != 0)
                return Ok($"Topic with id: {topic.Id} updated successfully");

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            var result = await _topicService.DeleteTopicAsync(id);

            if (result != 0)
                return Ok($"Topic with id: {id} deleted successfully");

            return NotFound($"Topic with id: {id} not found");
        }
    }


}
