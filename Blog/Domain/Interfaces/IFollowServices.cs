using Backend.Domain.Models;
using Blog.Infrastructure;

namespace Backend.Domain.Interfaces
{
    public interface IFollowServices
    {
        public Task<bool> Follow(FollowDTO userFollow);
        public Task<bool> Unfollow(FollowDTO userFollow);
        public Task<List<ApplicationUser>> GetAllFollower(string UserId );
        public Task<List<ApplicationUser>> GetAllFollowing(string UserId);  
    }
}