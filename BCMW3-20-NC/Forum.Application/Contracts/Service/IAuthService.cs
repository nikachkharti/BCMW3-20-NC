using Forum.API.Application.DTO.Auth;

namespace Forum.Application.Contracts.Service
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<string> RegisterAdmin(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

    }
}
