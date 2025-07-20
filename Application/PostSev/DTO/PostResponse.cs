namespace Application.PostSer.DTO;

public class PostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Message { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public bool Success { get; set; }
    
}