using Forum.API.Application.DTO.Auth;
using Forum.Application.Contracts.Service;
using Forum.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
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


        /// <summary>
        /// მომხმარებლის რეგისტრაცია
        /// </summary>
        [HttpPost("register")]
        [SwaggerRequestExample(typeof(RegistrationRequestDto), typeof(RegistrationRequestDtoExample))]
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

        /// <summary>
        /// ადმინის რეგისტრაცია
        /// </summary>
        //[Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        [SwaggerRequestExample(typeof(RegistrationRequestDto), typeof(RegistrationAdminRequestDtoExample))]
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


        /// <summary>
        /// ავტორიზაცია
        /// </summary>
        [HttpPost("login")]
        [SwaggerRequestExample(typeof(LoginRequestDto), typeof(LoginCustomerRequestDtoExample))]
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

        /// <summary>
        /// ბლოკის მოხსნა
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("unlock/{userId}")]
        public async Task<IActionResult> UnlockUserAccount([FromRoute] string userId)
        {
            var unlockResponse = await _authService.TryUnlockUserAccount(userId);

            return StatusCode(Convert.ToInt32(HttpStatusCode.OK), new CommonResponse()
            {
                IsSuccess = true,
                Message = unlockResponse ? "User unlocked successfully" : "User is already unlocked",
                Result = userId,
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// ბლოკის დადება
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("lock/{userId}")]
        public async Task<IActionResult> LockUserAccount([FromRoute] string userId)
        {
            var unlockResponse = await _authService.TryLockUserAccount(userId);

            return StatusCode(Convert.ToInt32(HttpStatusCode.OK), new CommonResponse()
            {
                IsSuccess = true,
                Message = unlockResponse ? "User locked successfully" : "User is already locked",
                Result = userId,
                StatusCode = HttpStatusCode.OK
            });
        }

    }
}
