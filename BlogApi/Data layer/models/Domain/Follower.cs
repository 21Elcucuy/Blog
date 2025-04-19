using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.DataLayer.Models.Domain
{
 
    public class Follower
    { 
          [ForeignKey("FollowerUser")]
          public string FollowerId { get; set; }
          [Required]
          public ApplicationUser FollowerUser  { get; set; } 


    }
}