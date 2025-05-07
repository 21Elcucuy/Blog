
using Microsoft.AspNetCore.Identity;

namespace Backend.Domain.Models
{
   public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime Created { get; set; }
        
        public List<UserFollow> Followers { get; set; }
        public List<UserFollow> Followings { get; set; }
       
    }



}