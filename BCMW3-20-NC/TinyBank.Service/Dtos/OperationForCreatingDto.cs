using TinyBank.Repository.Models.Enums;

namespace TinyBank.Service.Dtos;

public class OperationForCreatingDto
{
	 public OperationType OperationType { get; set; }
	 public String Currency { get; set; }
	 public Decimal Amount { get; set; }
	 public Int32 AccountId { get; set; }
	 public DateTime HappendAt { get; set; }
}
