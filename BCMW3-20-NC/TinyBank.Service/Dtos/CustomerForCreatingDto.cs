using TinyBank.Repository.Models.Enums;
using TinyBank.Service.Attributes;

namespace TinyBank.Service.Dtos;

public class CustomerForCreatingDto
{
    [CustomRequired]
    public String Name { get; set; }

    [CustomRequired]
    public String IdentityNumber { get; set; }

    [CustomRequired]
    public String PhoneNumber { get; set; }

    [CustomRequired]
    public String Email { get; set; }

    [CustomRequired]
    public CustomerType CustomerType { get; set; }
}
