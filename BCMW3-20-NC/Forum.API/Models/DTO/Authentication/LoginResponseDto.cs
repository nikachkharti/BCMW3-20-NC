namespace Forum.API.Models.DTO.Authentication
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
