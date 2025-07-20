using Application.CommentServices.DTO;

namespace Application.CommentServices.Interface;

public interface ICommentService
{
    public Task<CommentResponse> AddComment(AddCommentDTO commentDto ,string UserId, CancellationToken cancellationToken = default);
    public Task<List<CommentResponse>> GetAllCommentsForPost(Guid PostId,CancellationToken cancellationToken = default);
    public Task<CommentResponse> GetComment(Guid CommentId, CancellationToken cancellationToken = default);
    public Task<CommentResponse> DeleteComment(Guid CommentId, string UserId,CancellationToken cancellationToken = default);
    public Task<CommentResponse> UpdateComment(UpdateCommentDTO commentDto,string UserId, CancellationToken cancellationToken = default);
    public Task<List<CommentResponse>> GetAllCommentForUser(string userId, CancellationToken cancellationToken = default);
    
}