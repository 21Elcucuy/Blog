using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Backend.Domain.Models
{
    public class Post
    {
    [Key]
     public Guid id { get; set; }
     [Required]
     public string Title { get; set; }
     public string? SubTitle { get; set; }
     [Required]
     public string Content { get; set; }
     public DateTime Created { get; set; }
     public DateTime LastModified { get; set; }
     public int Likes { get; set; } 
     public string AuthorId { get; set; }
     public ApplicationUser Author { get; set; }
    
   

    }
}