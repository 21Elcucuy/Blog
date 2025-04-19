using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.DataLayer.Models.Domain
{
 
    public class Following

    {
        [ForeignKey("FollowerUser")]
          public string FollowingId { get; set; }
          public ApplicationUser FollowerUser { get; set; } 
 

       
    }
}