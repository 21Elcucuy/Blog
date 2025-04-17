using Microsoft.AspNetCore.Identity;

namespace BlogApi.Models.Domain
{
   public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime Created { get; set; }
    
    }



}