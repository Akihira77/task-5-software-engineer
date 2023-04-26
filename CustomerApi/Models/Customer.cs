using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Models;

public class Customer
{
	[Key]
	public int Id { get; set; }
	[Required]
	[StringLength(10, ErrorMessage = "Firstname must be a string with maximum length 10")]
	public string FirstName { get; set; } = string.Empty;
	[Required]
	[StringLength(10, ErrorMessage = "Lastname must be a string with maximum length 10")]
	public string LastName { get; set; } = string.Empty;
	[Required]
	[StringLength(25)]
	[EmailAddress(ErrorMessage = "Email string is not valid")]
	public string Email { get; set; } = string.Empty;
	[Required]
	[StringLength(50)]
	public string HomeAddress { get; set; } = string.Empty;
}
