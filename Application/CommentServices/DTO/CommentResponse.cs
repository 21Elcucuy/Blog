namespace Application.CommentServices.DTO;

public class CommentResponse
{
    public Guid CommentId { get; set; }
    public string UserName { get; set; }
    public string Content { get; set; }
}