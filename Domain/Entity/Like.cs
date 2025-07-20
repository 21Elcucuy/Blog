namespace Domain.Entity;

public class Like
{

    public Guid PostId { get; set; }
    public Post Post { get; set; } 
    public string UserId { get; set; }
    
}