namespace Backend.Application.DTOs
{
    public class PostDTO
    {
        public Guid id { get; set; }
        public string Title { get; set; }
        public string? SubTitle { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        
    }
}
