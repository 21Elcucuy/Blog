using Application.PostSer.DTO;
using Domain.Entity;

namespace Application.PostSev.Interface;

public interface IPostServices
{
    public Task<PostResponse> CreatePostAsync(AddPostDTO addpost , string userId);

    public Task<PostResponse> UpdatePostAsync(Guid postid, UpdatePostDTO post);
    
    public Task<PostResponse> DeletePostAsync(Guid postid);
    
    public Task<PostResponse> GetPostAsync(Guid postId);
    
    public Task<List<PostResponse>> GetAllPostAsync();
    
    public Task<List<PostResponse>> GetPostsByUserIdAsync(string userId);
}