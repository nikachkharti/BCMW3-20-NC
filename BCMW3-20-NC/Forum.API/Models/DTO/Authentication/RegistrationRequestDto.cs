namespace Forum.API.Models.DTO.Authentication
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
