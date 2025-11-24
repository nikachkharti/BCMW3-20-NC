using TinyBank.Repository.Models.Enums;

namespace TinyBank.Service.Dtos;

public class CustomerForCreatingDto
{
	 public String Name { get; set; }
	 public String IdentityNumber { get; set; }
	 public String PhoneNumber { get; set; }
	 public String Email { get; set; }
	 public CustomerType CustomerType { get; set; }
}
