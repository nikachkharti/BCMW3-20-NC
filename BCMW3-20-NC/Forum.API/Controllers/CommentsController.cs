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
            var result = await _commentRepository.GetAllAsync();

            if (result.TotalCount == 0)
                return NotFound();

            return Ok(result.Items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleComment(Guid id)
        {
            var comment = await _commentRepository.GetAsync(x => x.Id == id, includeProperties: "Topic");

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewComment([FromBody] Comment comment)
        {
            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] Comment comment)
        {
            var commentToUpdate = await _commentRepository.GetAsync(x => x.Id == comment.Id);

            if (commentToUpdate == null)
                return NotFound();

            _commentRepository.Update(comment);
            await _commentRepository.SaveAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var commmentToDelete = await _commentRepository.GetAsync(x => x.Id == id);

            if (commmentToDelete == null)
                return NotFound();

            _commentRepository.Remove(commmentToDelete);
            await _commentRepository.SaveAsync();

            return NoContent();
        }
    }

}
