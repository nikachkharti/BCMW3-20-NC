using Forum.API.Application.DTO.Comments;
using Forum.API.Application.DTO.Topics;
using Forum.Application.Contracts.Service;
using Forum.Application.Features.Topics.Commands;
using Forum.Application.Features.Topics.Queries;
using Forum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Forum.API.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly ICommentService _commentService;
        private readonly IMediator _mediator;

        public TopicsController(ITopicService topicService, ICommentService commentService, IMediator mediator)
        {
            _topicService = topicService;
            _commentService = commentService;
            _mediator = mediator;
        }

        /// <summary>
        /// ყველა პოსტი
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTopics(
            [FromQuery] int? pageNumber = 1,
            [FromQuery] int? pageSize = 10)
        {
            var query = new GetAllTopicsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query, cancellationToken: default);

            return Ok(new CommonResponse
            {
                Message = "Topics retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = new
                {
                    Topics = result.Topics,
                    TotalCount = result.TotalCount
                }
            });
        }


        /// <summary>
        /// პოსტი დეტალურად
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleTopic(Guid id)
        {
            var query = new GetTopicDetailsQuery(id);
            var result = await _mediator.Send(query, cancellationToken: default);

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
            var command = new CreateTopicCommand(model);
            var result = await _mediator.Send(command, cancellationToken: default);

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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateTopic([FromBody] TopicForUpdatingDto model)
        {
            var command = new UpdateTopicCommand(model);
            var result = await _mediator.Send(command, cancellationToken: default);

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
        public async Task<IActionResult> DeleteTopic([FromRoute] Guid id)
        {
            var command = new DeleteTopicCommand(id);
            var result = await _mediator.Send(command, cancellationToken: default);

            return Ok(new CommonResponse
            {
                Message = $"Topic with id: {id} deleted successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }


        /// <summary>
        /// არსებულ პოსტზე კომენტარის დამატება.
        /// </summary>
        [HttpPost("comments")]
        [Authorize]
        public async Task<IActionResult> AddComent([FromForm] CommentForCreatingDto model)
        {
            var result = await _commentService.AddNewCommentAsync(model);

            return StatusCode(StatusCodes.Status201Created, new CommonResponse
            {
                Message = "Comment added successfully",
                StatusCode = HttpStatusCode.Created,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// არსებულ პოსტზე კომენტარის განახლება.განაახლოს მხოლოდ ავტორმა
        /// </summary>
        [HttpPatch("{topicId}/comments")]
        [Authorize]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid topicId, [FromForm] CommentForUpdatingDto model)
        {
            var result = await _commentService.UpdateCommentAsync(model);

            return Ok(new CommonResponse
            {
                Message = $"Comment with id: {model.Id} updated successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }


        /// <summary>
        /// არსებულ პოსტზე კომენტარის წაშლა. წაშალოს მხოლოდ ავტორმა
        /// </summary>
        [HttpDelete("{topicId}/comments/{commentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComent([FromRoute] Guid topicId, [FromRoute] Guid commentId)
        {
            var result = await _commentService.DeleteCommentAsync(commentId);

            return Ok(new CommonResponse
            {
                Message = $"Comment with id: {commentId} deleted successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }


    }



}
