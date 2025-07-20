namespace Application.CommentServices.DTO;

public class AddCommentDTO
{
    public Guid PostId { get; set; }
    public string Content { get; set; }
}