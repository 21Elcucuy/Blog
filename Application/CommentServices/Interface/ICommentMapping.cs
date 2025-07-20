using Application.CommentServices.DTO;
using Domain.Entity;

namespace Application.CommentServices.Interface;

public interface ICommentMapping
{
    public Comment ToComment(AddCommentDTO comment ,string userId);
    public Comment ToComment(UpdateCommentDTO comment );
    public Task<CommentResponse> ToCommentResponse(Comment comment, CancellationToken cancellationToken = default);
    public Task<List<CommentResponse>> ToListOfCommentResponse(List<Comment> comments , CancellationToken cancellationToken = default);
    
}