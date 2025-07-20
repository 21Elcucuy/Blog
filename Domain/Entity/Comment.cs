namespace Domain.Entity;

public class Comment
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; } 
    public string UserId { get; set; }
    public string Content { get; set; }
}