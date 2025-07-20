using Domain.Entity;
using Domain.Interface;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class SqlLikeRepository : ILikeRepository
{
    private readonly BlogDbContext _context;

    public SqlLikeRepository(BlogDbContext context)
    {
        _context = context;
    }
    public async Task<Like> AddLikeAsync(Like like , CancellationToken cancellationToken = default)
    {
        var Exist = await _context.Likes.AnyAsync(x=> x.UserId == like.UserId && x.PostId == like.PostId, cancellationToken);
        if (Exist)
        {
            return null;
        }
        await _context.Likes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return like;
    }

    public async  Task<Like> DeleteLikeAsync(Like like , CancellationToken cancellationToken = default)
    {
        var Exist = await _context.Likes.AnyAsync(x=> x.UserId == like.UserId && x.PostId == like.PostId, cancellationToken);
        if (!Exist)
        {
            return null;
        }
        _context.Likes.Remove(like);
        await _context.SaveChangesAsync(cancellationToken);
        return like;
    }

    public async Task<List<Like>> GetAllUserLikeAsync(string userId , CancellationToken cancellationToken = default)
    {
        var UserLike = await _context.Likes.Where(x => x.UserId ==userId).ToListAsync(cancellationToken);
        return UserLike;
    }

    public async Task<List<Like>> GetAllPostLikeAsync(Guid postId , CancellationToken cancellationToken = default)
    {
        var UserLike = await _context.Likes.Where(x => x.PostId == postId ).ToListAsync(cancellationToken);
        return UserLike;
    }
}