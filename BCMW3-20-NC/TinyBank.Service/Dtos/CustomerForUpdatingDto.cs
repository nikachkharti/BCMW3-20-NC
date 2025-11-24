using TinyBank.Repository.Models.Enums;

namespace TinyBank.Service.Dtos;

public class CustomerForUpdatingDto
{
	 public Int32 Id { get; set; }
	 public String Name { get; set; }
	 public String IdentityNumber { get; set; }
	 public String PhoneNumber { get; set; }
	 public String Email { get; set; }
	 public CustomerType CustomerType { get; set; }
}
