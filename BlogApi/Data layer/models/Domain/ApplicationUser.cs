using BlogApi.DataLayer.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Models.Domain
{
   public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime Created { get; set; }


        public ICollection<Follower> Followers { get; set; }
    
        public ICollection<Following> Followings { get; set; }

    }



}