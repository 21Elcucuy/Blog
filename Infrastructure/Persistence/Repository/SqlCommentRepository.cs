using Domain.Entity;
using Domain.Interface;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class SqlCommentRepository : ICommentRepository
{
    private readonly BlogDbContext _context;

    public SqlCommentRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateComment(Comment comment ,CancellationToken cancellationToken = default)
    {
        var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == comment.PostId, cancellationToken);
        if (post == null)
        {
            return null;
        }
       await _context.Comments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return comment;
    }

    public async Task<List<Comment>> GetAllCommentsForPost(Guid postId , CancellationToken cancellationToken = default)
    {
        var CommentList = await  _context.Comments.Where(x => x.PostId == postId).ToListAsync(cancellationToken);
        
        return CommentList;
    }
    public async Task<Comment> UpdateComment(Comment comment, string UserId , CancellationToken cancellationToken = default)
    {
        var OriginalComment = _context.Comments.FirstOrDefault(x => x.Id == comment.Id);
        if (OriginalComment is null || OriginalComment.UserId != UserId)
        {
            return null;
        }
        OriginalComment.Content = comment.Content;
        await _context.SaveChangesAsync(cancellationToken);
        return OriginalComment;
    }

    public async Task<Comment> DeleteComment(Guid commentId, string UserId ,  CancellationToken cancellationToken = default)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId, cancellationToken);
        if (comment is null || comment.UserId != UserId)
        {
            return null;
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return comment;
    }

    public async Task<List<Comment>> GetAllCommentsForUser(string  userId, CancellationToken cancellationToken = default)
    {
        var Comment = await _context.Comments.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        return Comment;
    }

    public async Task<Comment> GetComment(Guid CommentId, CancellationToken cancellationToken = default)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == CommentId, cancellationToken);
        return comment;
    }
}