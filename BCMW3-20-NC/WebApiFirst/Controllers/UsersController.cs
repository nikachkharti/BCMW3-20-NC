using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiFirst.Services;

namespace WebApiFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public UsersController(INotificationService notificationService)
        {
            _notificationService= notificationService;
        }


        [HttpGet]
        public void Test()
        {
            _notificationService.Send(); //1
            _notificationService.Send(); //1
            _notificationService.Send(); //1
        }

        [HttpPost]
        public void Test2()
        {
            _notificationService.Send(); //2
        }
    }
}
