using Forum.API.Models.DTO.Authentication;

namespace Forum.API.Services
{
    public interface IAuthService
    {
        Task Register(RegistrationRequestDto registrationRequestDto);
        Task RegisterAdmin(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
