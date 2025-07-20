using Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entity.Identity;

public class ApplicationUser : IdentityUser
{
  public string FirstName { get; set; }
  
  public string LastName { get; set; }
  
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  

}