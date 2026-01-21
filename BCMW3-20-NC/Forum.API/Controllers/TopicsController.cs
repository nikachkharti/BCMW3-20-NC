using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllTopics()
        {
            return Ok();
        }
    }
}
