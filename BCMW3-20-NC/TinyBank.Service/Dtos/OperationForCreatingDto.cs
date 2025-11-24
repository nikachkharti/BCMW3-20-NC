using TinyBank.Repository.Models.Enums;
using TinyBank.Service.Attributes;

namespace TinyBank.Service.Dtos;

public class OperationForCreatingDto
{
    [CustomRequired]
    public OperationType OperationType { get; set; }

    [CustomRequired]
    [CustomMinLength(3)]
    [CustomMaxLength(3)]
    public String Currency { get; set; }

    [CustomRequired]
    public Decimal Amount { get; set; }

    [CustomRequired]
    public Int32 AccountId { get; set; }

    [CustomRequired]
    public DateTime HappendAt { get; set; }
}
