namespace Domain.Entity;

public class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorId { get; set; } 
    public ICollection<Like> Likes { get; set; }
    public ICollection<Comment> Comments { get; set; }
    
}