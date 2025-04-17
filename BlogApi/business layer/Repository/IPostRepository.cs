using BlogApi.Models;
using BlogApi.Models.Domain;

namespace BlogApi.Repository
{
    public interface IPostRepository
    {
         Task<Post> CreatePostAsync(Post post);
         Task<List<Post>> GetAllPostsAsync();
         Task<Post> GetAtAsync(Guid id);
         Task<Post> UpdatePostAsync( Guid id,Post post);
         Task<Post> DeletePostAsync(Guid id);

    }
}