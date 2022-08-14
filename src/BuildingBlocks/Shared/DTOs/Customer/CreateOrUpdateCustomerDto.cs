using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Customer;

public class CreateOrUpdateCustomerDto
{
    [Required]
    public  int Id { get; set; }
    
    [Required]
    [MaxLength(100, ErrorMessage = "Maximum length for First Name is 100 characters.")]
    public string FirstName { get; set; }
    
    [MaxLength(100, ErrorMessage = "Maximum length for Product Summary is 100 characters.")]
    public string LastName { get; set; }
}