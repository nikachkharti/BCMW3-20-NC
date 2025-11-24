using TinyBank.Repository.Models.Enums;
using TinyBank.Service.Attributes;

namespace TinyBank.Service.Dtos;

public class CustomerForCreatingDto
{
    [CustomRequired]
    [CustomMaxLength(50)]
    public String Name { get; set; }

    [CustomRequired]
    [CustomMinLength(11)]
    [CustomMaxLength(11)]
    public String IdentityNumber { get; set; }

    [CustomRequired]
    [CustomMaxLength(50)]
    public String PhoneNumber { get; set; }

    [CustomRequired]
    [CustomMaxLength(50)]
    public String Email { get; set; }

    [CustomRequired]
    public CustomerType CustomerType { get; set; }
}
