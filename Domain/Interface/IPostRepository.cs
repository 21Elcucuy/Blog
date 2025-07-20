using Domain.Entity;

namespace Domain.Interface;

public interface IPostRepository
{
    
    public Task<Post> CreatePostAsync(Post post);
    public Task<Post> UpdatePostAsync(Guid postid,Post post);
 
    public Task<Post> DeletePostAsync(Guid postid);
    public Task<Post> GetPostAsync(Guid postId);
    public Task<List<Post>> GetAllPostAsync();
    public Task<List<Post>> GetPostsByUserIdAsync(string userId);
}