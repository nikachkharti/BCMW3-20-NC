using Forum.API.Models;
using Forum.API.Models.DTO.Auth;
using Forum.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Forum.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var customerId = await _authService.Register(model);

            return StatusCode(Convert.ToInt32(HttpStatusCode.OK), new CommonResponse()
            {
                IsSuccess = true,
                Message = "Customer registered successfully",
                Result = customerId,
                StatusCode = HttpStatusCode.OK
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationRequestDto model)
        {
            var adminId = await _authService.RegisterAdmin(model);

            return StatusCode(Convert.ToInt32(HttpStatusCode.OK), new CommonResponse()
            {
                IsSuccess = true,
                Message = "Admin registered successfully",
                Result = adminId,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);

            return StatusCode(Convert.ToInt32(HttpStatusCode.OK), new CommonResponse()
            {
                IsSuccess = true,
                Message = "User authorized successfully",
                Result = loginResponse.Token,
                StatusCode = HttpStatusCode.OK
            });
        }

    }
}
