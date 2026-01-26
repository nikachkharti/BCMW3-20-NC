using Forum.API.Entities;
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
            var result = await _topicService.GetAllTopicsAsync(pageNumber, pageSize);

            if (result.Item1.Count > 0)
                return Ok(new
                {
                    Topics = result.Item1,
                    TopicsCount = result.totalCount
                });

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleTopic(Guid id)
        {
            var result = await _topicService.GetOpicDetailsAsync(id);

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
        public async Task<IActionResult> UpdateTopic([FromBody] Topic topic)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            throw new NotImplementedException();
        }
    }


}
