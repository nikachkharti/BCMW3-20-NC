using TinyBank.Repository.Models.Enums;
using TinyBank.Service.Attributes;

namespace TinyBank.Service.Dtos;

public class AccountForCreatingDto
{
    [CustomRequired]
    [CustomMinLength(22)]
    [CustomMaxLength(22)]
    public String Iban { get; set; }

    [CustomRequired]
    [CustomMinLength(3)]
    [CustomMaxLength(3)]
    public String Currency { get; set; }

    [CustomRequired]
    public Decimal Balance { get; set; }

    [CustomRequired]
    public Int32 CustomerId { get; set; }
    public String Destination { get; set; }
}
