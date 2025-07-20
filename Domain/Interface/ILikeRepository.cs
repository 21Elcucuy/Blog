using Domain.Entity;

namespace Domain.Interface;

public interface ILikeRepository
{
    public Task<Like> AddLikeAsync(Like like , CancellationToken cancellationToken = default);
    public Task<Like> DeleteLikeAsync(Like like , CancellationToken cancellationToken = default);
    public Task<List<Like>> GetAllUserLikeAsync(string userId , CancellationToken cancellationToken = default);
    public Task<List<Like>> GetAllPostLikeAsync(Guid postId , CancellationToken cancellationToken = default);
}