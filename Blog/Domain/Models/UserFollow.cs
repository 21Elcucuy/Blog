namespace Backend.Domain.Models;

public class UserFollow
{
     public string FollowedId { get; set; }
     public ApplicationUser Follower { get; set; }
     
     public string FollowingId { get; set; }
     public ApplicationUser Following { get; set; }
     
     
}