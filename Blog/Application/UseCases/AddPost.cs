namespace Backend.Application.UseCases;

public class AddPost
{
    public string Title { get; set; }
    public string? SubTitle { get; set; }
    public string Content { get; set; }
    public string AuthorId { get; set; }
}