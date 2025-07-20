using Application.PostSer.DTO;
using Domain.Entity;
using Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.PostSev.Extenstion;

public class PostMapping 
{
    private readonly UserManager<ApplicationUser> _userManager;

    public PostMapping(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public Post AddPostToPost(AddPostDTO addpost ,string userId )
    {
        return new Post()
        {
            Title = addpost.Title,
            Content = addpost.Content,
            AuthorId = userId,
        };
    }

    public async Task<PostResponse> PostToPostResponse(Post post)
    {
        if (post == null)
        {
            return new PostResponse()
            {
                Success = false,
                Message = "There is Error",
            };
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == post.AuthorId);
        return new PostResponse()
        {
            Id = post.Id,
            Success = true,
            Message = "Success",
            Title = post.Title,
            Content = post.Content,
            AuthorId = post.AuthorId,
            AuthorName = user.UserName,
        };
    }

    public Post UpdatePostToPost(UpdatePostDTO updatePost)
    {
        return new Post()
        {
            Title = updatePost.Title,
            Content = updatePost.Content,
         
        };
    }

    public async  Task<List<PostResponse>> ListPostToListPostResponse(List<Post> posts)
    {
        var postReponse = new List<PostResponse>();
        foreach (var post in posts)
        {
            postReponse.Add(await PostToPostResponse(post));
        }
        return postReponse;
    }
}