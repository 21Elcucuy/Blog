using Domain.Entity;

namespace Domain.Interface;

public interface ICommentRepository
{
    public Task<Comment> CreateComment(Comment comment, CancellationToken cancellationToken = default);
    public Task<List<Comment>> GetAllCommentsForPost(Guid postId, CancellationToken cancellationToken = default);
    public Task<Comment> UpdateComment(Comment comment, string UserId , CancellationToken cancellationToken = default);
    public Task<Comment> DeleteComment(Guid commentId, string UserId ,CancellationToken cancellationToken = default);
    public Task<List<Comment>> GetAllCommentsForUser(string  userId, CancellationToken cancellationToken = default);
    public Task<Comment> GetComment(Guid CommentId, CancellationToken cancellationToken = default);
}