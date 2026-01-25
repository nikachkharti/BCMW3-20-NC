using Forum.API.Entities;
using Forum.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var result = await _commentRepository.GetAllCommentsAsync();

            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleComment(Guid id)
        {
            var comment = await _commentRepository.GetSingleCommentAsync(id);

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewComment([FromBody] Comment comment)
        {
            await _commentRepository.AddNewCommentAsync(comment);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] Comment comment)
        {
            await _commentRepository.UpdateNewCommentAsync(comment);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var deleted = await _commentRepository.DeleteSingleCommentAsync(id);

            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }
    }

}
