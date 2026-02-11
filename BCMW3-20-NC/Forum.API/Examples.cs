using Forum.API.Application.DTO.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace Forum.API
{
    public sealed record LoginCustomerRequestDtoExample : IExamplesProvider<LoginRequestDto>
    {
        public LoginRequestDto GetExamples()
        {
            return new LoginRequestDto() { UserName = "admin@gmail.com", Password = "Admin123!" };
        }
    }

    public sealed record RegistrationRequestDtoExample : IExamplesProvider<RegistrationRequestDto>
    {
        public RegistrationRequestDto GetExamples()
        {
            return new RegistrationRequestDto()
            {
                Email = "rezo@gmail.com",
                FullName = "Revaz Revazishvili",
                Password = "Rezo123!"
            };
        }
    }
    public sealed record RegistrationAdminRequestDtoExample : IExamplesProvider<RegistrationRequestDto>
    {
        public RegistrationRequestDto GetExamples()
        {
            return new RegistrationRequestDto()
            {
                Email = "admin2@gmail.com",
                FullName = "Admin2 Admin2",
                Password = "Admin123!"
            };
        }
    }


}
