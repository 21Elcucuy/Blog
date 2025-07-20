using Application.CommentServices.DTO;
using Application.CommentServices.Interface;
using Domain.Interface;

namespace Application.CommentServices.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;
    private readonly ICommentMapping _mapping;

    public CommentService(ICommentRepository repository , ICommentMapping mapping)
    {
        _repository = repository;
        _mapping = mapping;
    }
    public async Task<CommentResponse> AddComment(AddCommentDTO commentDto, string UserId, CancellationToken cancellationToken = default)
    {
       var comment =  await _repository.CreateComment(_mapping.ToComment(commentDto, UserId), cancellationToken);
         return await  _mapping.ToCommentResponse(comment , cancellationToken); 
       
    }

    public async Task<List<CommentResponse>> GetAllCommentsForPost(Guid PostId, CancellationToken cancellationToken = default)
    {
       var comment =  await _repository.GetAllCommentsForPost(PostId, cancellationToken);
       return await _mapping.ToListOfCommentResponse(comment, cancellationToken);
    }

    public async  Task<CommentResponse> GetComment(Guid CommentId, CancellationToken cancellationToken = default)
    {
             var comment = await _repository.GetComment(CommentId,cancellationToken);
             return await _mapping.ToCommentResponse(comment, cancellationToken);
    }

    public async Task<CommentResponse> DeleteComment(Guid CommentId, string UserId, CancellationToken cancellationToken = default)
    {
        var comment  = await _repository.DeleteComment(CommentId, UserId,cancellationToken);
        return await _mapping.ToCommentResponse(comment, cancellationToken);
    }

   

    public async Task<CommentResponse> UpdateComment(UpdateCommentDTO commentDto, string UserId, CancellationToken cancellationToken = default)
    {
        
        var comment = _mapping.ToComment(commentDto);
        var response = await _repository.UpdateComment(comment,UserId,cancellationToken);
        return await _mapping.ToCommentResponse(response, cancellationToken);
    }

    public async  Task<List<CommentResponse>> GetAllCommentForUser(string userId, CancellationToken cancellationToken = default)
    {
        var comment = await  _repository.GetAllCommentsForUser(userId, cancellationToken);
        
        return await _mapping.ToListOfCommentResponse(comment, cancellationToken);
        
    }
}