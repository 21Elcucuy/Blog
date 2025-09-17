using System.ComponentModel.DataAnnotations;

namespace Application.Authentication.DTO;
public class LoginRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    [Required]
    [MinLength(6, ErrorMessage = "Invalid Password")]
    public string Password { get; set; }    
    
}