using Backend.Domain.Interfaces;
using Backend.Domain.Models;
using Backend.Infrastructure.Persistence.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.ExternalServices
{
public class FollowServices : IFollowServices
{
    private readonly AuthDbContext _context;


    public FollowServices(AuthDbContext context )
    {
        _context = context;
    }
    public async Task<bool> Follow(FollowDTO userFollow)
    {
        if(await CheckUserIsNull(userFollow))
            return false;
        
        
        if (await IsFollowing(userFollow))
                 return false;
        UserFollow userFollows = new UserFollow()
             {
                 FollowedId = userFollow.FollowedId,
                 FollowingId = userFollow.FollowingId,
             };
             _context.UserFollows.Add(userFollows);
             await _context.SaveChangesAsync();
           
        
        return true;
    }

    public async  Task<bool> Unfollow(FollowDTO userFollow)
    {
        if(await CheckUserIsNull(userFollow))
            return false;
        if (!await IsFollowing(userFollow))
            return false;
        UserFollow userFollows = new UserFollow()
        {
            FollowedId = userFollow.FollowedId,
            FollowingId = userFollow.FollowingId,
        };
        await _context.UserFollows.FindAsync(userFollow);
        _context.UserFollows.Remove(userFollows);
        await _context.SaveChangesAsync();
        return true;

    }

    public  async  Task<List<ApplicationUser>> GetAllFollower(string UserId)
    {
      return  await  _context.UserFollows.Where(p => p.FollowingId == UserId).Select(p => p.Follower ).ToListAsync();

    }

    public async Task<List<ApplicationUser>> GetAllFollowing(string UserId)
    {
        return  await  _context.UserFollows.Where(p => p.FollowedId == UserId).Select(p => p.Following ).ToListAsync();
    }

    private async Task<bool> CheckUserIsNull(FollowDTO userFollow)
    {
        if (await _context.Users.FirstOrDefaultAsync(p => p.Id == userFollow.FollowedId) is null ||
            await _context.Users.FirstOrDefaultAsync(f => f.Id == userFollow.FollowingId) is null)
            return true;
        return false;

    }

    private async Task<bool> IsFollowing(FollowDTO userFollow)
    {
        return  await _context.UserFollows
            .AnyAsync(fo => fo.FollowedId == userFollow.FollowedId && fo.FollowingId == userFollow.FollowingId);
    }
}

}