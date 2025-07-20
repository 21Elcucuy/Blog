using Application.LikeServices.DTO;
using Application.LikeServices.Interface;
using Domain.Entity;
using Domain.Interface;

namespace Application.LikeServices.Services;

public class LikeService: ILikeService
{
    private readonly ILikeRepository _repository;
    private readonly ILikeMapping _mapping;

    public LikeService(ILikeRepository repository , ILikeMapping mapping)
    {
        _repository = repository;
        _mapping = mapping;
    }
    public async Task<LikeResponse> AddLikeAsync(AddLikeDTO addlikeDTO, string UserId, CancellationToken cancellationToken = default)
    { 
        var like = _mapping.ToLike(addlikeDTO, UserId);
        var response = await _repository.AddLikeAsync(like, cancellationToken);
        return await _mapping.ToLikeResponse(response, cancellationToken);
    }

    public async Task<LikeResponse> DeleteLikeAsync(DeleteLikeDTO deletelikeDTO, string UserId, CancellationToken cancellationToken = default)
    {
        var like = _mapping.ToLike(deletelikeDTO, UserId);
        var response = await _repository.DeleteLikeAsync(like, cancellationToken);
        return await _mapping.ToLikeResponse(response, cancellationToken);
    }

    public async Task<List<LikeResponse>> GetAllLikesForUserAsync(string UserId, CancellationToken cancellationToken = default)
    {
        var response = await _repository.GetAllUserLikeAsync(UserId ,cancellationToken);
        return await  _mapping.ToListOfLikeResponse(response,cancellationToken);
    }

    public async Task<List<LikeResponse>> GetAllLikesForPostAsync(Guid PostId, CancellationToken cancellationToken = default)
    {
        var response = await _repository.GetAllPostLikeAsync(PostId, cancellationToken);
        return await _mapping.ToListOfLikeResponse(response,cancellationToken);
        
    }
}