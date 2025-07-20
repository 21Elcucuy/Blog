using Application.PostSer.DTO;
using Application.PostSev.Extenstion;
using Application.PostSev.Interface;
using Domain.Entity;
using Domain.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Application.PostSer.Services;

public class PostServices : IPostServices
{
    private readonly PostMapping _postMapping;
    private readonly IPostRepository _postRepository;

    public PostServices(PostMapping postMapping ,IPostRepository postRepository)
    {
        _postMapping = postMapping;
        _postRepository = postRepository;
    }
    public async Task<PostResponse> CreatePostAsync(AddPostDTO addpost , string userId)
    {
       var Response = await _postRepository.CreatePostAsync(_postMapping.AddPostToPost(addpost, userId));
        return await _postMapping.PostToPostResponse(Response);
    }

    public async Task<PostResponse> UpdatePostAsync(Guid postid, UpdatePostDTO post)
    {
       var Response = await _postRepository.UpdatePostAsync(postid,_postMapping.UpdatePostToPost(post));
       return await _postMapping.PostToPostResponse(Response);
    }

    public async Task<PostResponse> DeletePostAsync(Guid postid)
    {
        var Response = await  _postRepository.DeletePostAsync(postid);
        return await _postMapping.PostToPostResponse(Response);
    }

    public async Task<PostResponse> GetPostAsync(Guid postId)
    {
        var Response  = await _postRepository.GetPostAsync(postId);
        return await _postMapping.PostToPostResponse(Response);
    }

    public async Task<List<PostResponse>> GetAllPostAsync()
    {
       var Response =  await _postRepository.GetAllPostAsync(); 
       return await _postMapping.ListPostToListPostResponse(Response);
    }

    public async Task<List<PostResponse>> GetPostsByUserIdAsync(string userId)
    {
        var Response =  await _postRepository.GetPostsByUserIdAsync(userId);
        return await _postMapping.ListPostToListPostResponse(Response);
    }
}