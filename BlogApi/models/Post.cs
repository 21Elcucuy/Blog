using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
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
    }
}