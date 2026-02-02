using Forum.API.Models.DTO.Auth;

namespace Forum.API.Services
{
    public interface IAuthService
    {
        Task Register(RegistrationRequestDto registrationRequestDto);
        Task RegisterAdmin(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

    }
}
