using Forum.API.Entities;
using Forum.API.Models;
using Forum.API.Models.DTO.Topics;
using Forum.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;


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

            var response = new CommonResponse
            {
                Message = "Topics retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = new
                {
                    Topics = result.Topics,
                    TotalCount = result.TotalCount
                }
            };

            return StatusCode(Convert.ToInt32(response.StatusCode), response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleTopic(Guid id)
        {
            var result = await _topicService.GetTopicDetailsAsync(id);

            var response = new CommonResponse
            {
                Message = "Topic retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            };

            return StatusCode(Convert.ToInt32(response.StatusCode), response);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTopic([FromBody] TopicForCreatingDto model)
        {
            var result = await _topicService.AddNewTopicAsync(model);

            var response = new CommonResponse
            {
                Message = "Topic added successfully",
                StatusCode = HttpStatusCode.Created,
                IsSuccess = true,
                Result = result
            };

            return StatusCode(Convert.ToInt32(response.StatusCode), response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromBody] TopicForUpdatingDto topic)
        {
            var result = await _topicService.UpdateNewTopicAsync(topic);

            var response = new CommonResponse
            {
                Message = $"Topic with id: {topic.Id} updated successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            };

            return StatusCode(Convert.ToInt32(response.StatusCode), response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            var result = await _topicService.DeleteTopicAsync(id);

            var response = new CommonResponse
            {
                Message = $"Topic with id: {id} deleted successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            };

            return StatusCode(Convert.ToInt32(response.StatusCode), response);
        }
    }


}
