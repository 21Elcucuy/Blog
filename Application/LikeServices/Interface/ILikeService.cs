using Application.LikeServices.DTO;

namespace Application.LikeServices.Interface;

public interface ILikeService
{ 
    public Task<LikeResponse> AddLikeAsync(AddLikeDTO addlikeDTO ,string UserId , CancellationToken cancellationToken = default);
    public Task<LikeResponse> DeleteLikeAsync(DeleteLikeDTO deletelikeDTO, string UserId , CancellationToken cancellationToken = default);
    public Task<List<LikeResponse>> GetAllLikesForUserAsync(string UserId , CancellationToken cancellationToken = default);
    public Task<List<LikeResponse>> GetAllLikesForPostAsync(Guid PostId , CancellationToken cancellationToken = default);
    
}