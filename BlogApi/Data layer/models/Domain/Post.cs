using System.ComponentModel.DataAnnotations;
using BlogApi.DataLayer.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Models.Domain
{
    public class Post
    {
    [Key]
     public Guid id { get; set; }
     [Required]
     public string Title { get; set; }
     public string? subTitle { get; set; }
     [Required]
     public string Content { get; set; }
     public DateTime Created { get; set; }
     public DateTime LastModified { get; set; }
     public Guid AuthorId { get; set; }
     public int Likes { get; set; }
      
    
    //navigation
    public Author Author { get; set; }

    }
}