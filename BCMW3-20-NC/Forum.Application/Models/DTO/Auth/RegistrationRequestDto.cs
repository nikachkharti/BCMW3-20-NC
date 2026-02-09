namespace Forum.API.Application.DTO.Auth
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}
