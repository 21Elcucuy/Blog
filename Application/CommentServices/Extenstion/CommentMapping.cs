using Application.CommentServices.DTO;
using Application.CommentServices.Interface;
using Domain.Entity;
using Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.CommentServices.Extenstion;

public class CommentMapping : ICommentMapping
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CommentMapping(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public Comment ToComment(AddCommentDTO commentDto ,string userId)
    {
        var comment = new Comment()
        {
            UserId = userId,
            Content = commentDto.Content,
            PostId = commentDto.PostId,
        };
        return comment;

    }

    public Comment ToComment(UpdateCommentDTO comment)
    {
        var Comment = new Comment()
        {
            Id = comment.CommentId,
            Content = comment.Content,
        };
        return Comment;
    }

    public async  Task<CommentResponse> ToCommentResponse(Comment comment, CancellationToken cancellationToken = default)
    {
        if (comment == null)
        {
            return null;
        }
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == comment.UserId , cancellationToken);  
        CommentResponse response = new CommentResponse()
        {
            CommentId = comment.Id,
            Content = comment.Content,
            UserName = user?.UserName,
        };
        return response;
    }

    public async Task<List<CommentResponse>> ToListOfCommentResponse(List<Comment> comments,CancellationToken cancellationToken = default)
    { 
        var userIds = comments.Select(x => x.UserId).Distinct().ToList();
        var users = await _userManager.Users.Where(x => userIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
        
        
        List<CommentResponse> response = new List<CommentResponse>();
        foreach (var comment in comments)
        {
         response.Add(new CommentResponse()
         {
             CommentId = comment.Id,
             Content = comment.Content,
             UserName = users.TryGetValue(comment.UserId , out var user) ? user.UserName : "Unknown",
         });
         
        }

        return response;


    }
}