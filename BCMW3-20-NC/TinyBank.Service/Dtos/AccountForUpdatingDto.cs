using TinyBank.Repository.Models.Enums;
using TinyBank.Service.Attributes;

namespace TinyBank.Service.Dtos;

public class AccountForUpdatingDto
{
    [CustomRequired]
    public Int32 Id { get; set; }

    [CustomRequired]
    public String Iban { get; set; }

    [CustomRequired]
    public String Currency { get; set; }

    [CustomRequired]
    public Decimal Balance { get; set; }

    [CustomRequired]
    public Int32 CustomerId { get; set; }

    public String Destination { get; set; }
}
