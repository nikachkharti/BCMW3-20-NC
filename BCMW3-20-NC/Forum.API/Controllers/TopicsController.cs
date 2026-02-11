using Forum.API.Application.DTO.Topics;
using Forum.Application.Contracts.Service;
using Forum.Application.Models;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// ყველაა პოსტი
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTopics(
            [FromQuery] int? pageNumber = 1,
            [FromQuery] int? pageSize = 10)
        {
            var (topics, totalCount) = await _topicService.GetAllTopicsAsync(pageNumber, pageSize);

            return Ok(new CommonResponse
            {
                Message = "Topics retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = new
                {
                    Topics = topics,
                    TotalCount = totalCount
                }
            });
        }


        /// <summary>
        /// პოსტი დეტალურად
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleTopic(Guid id)
        {
            var result = await _topicService.GetTopicDetailsAsync(id);

            return Ok(new CommonResponse
            {
                Message = "Topic retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// ახალი პოსტი
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewTopic([FromForm] TopicForCreatingDto model)
        {
            var result = await _topicService.AddNewTopicAsync(model);

            return StatusCode(StatusCodes.Status201Created, new CommonResponse
            {
                Message = "Topic added successfully",
                StatusCode = HttpStatusCode.Created,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// არსებული პოსტის განახლება
        /// </summary>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateTopic([FromBody] TopicForUpdatingDto model)
        {
            var result = await _topicService.UpdateTopicAsync(model);

            return Ok(new CommonResponse
            {
                Message = $"Topic with id: {model.Id} updated successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// არსებული პოსტის წაშლა
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            var result = await _topicService.DeleteTopicAsync(id);

            return Ok(new CommonResponse
            {
                Message = $"Topic with id: {id} deleted successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }
    }



}
