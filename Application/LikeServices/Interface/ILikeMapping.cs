using Application.LikeServices.DTO;
using Domain.Entity;

namespace Application.LikeServices.Interface;

public interface ILikeMapping
{
    public Like ToLike(AddLikeDTO addlikeDTO, string UserId);
    public Like ToLike(DeleteLikeDTO addlikeDTO, string UserId);
    public Task<LikeResponse> ToLikeResponse(Like like , CancellationToken cancellationToken = default);
    public Task<List<LikeResponse>> ToListOfLikeResponse(List<Like> likes,CancellationToken cancellationToken = default);
}