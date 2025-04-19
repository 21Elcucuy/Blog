using BlogApi.DataLayer.Models.Domain;
using BlogApi.Models.Domain;

namespace BlogApi.BusinessLayer.Repository
{
    public interface IFollowRepository
    {
        // Task<bool> AddFolloweer(UserFollow userFollow);
        // Task<bool> RemoveFollower(UserFollow userFollow);

        // Task<bool> AddFollowing(UserFollow userFollow);
        // Task<bool> RemoveFollowing(UserFollow userFollow);   
        Task<bool> Follow(UserFollow userFollow);
        Task<bool> Unfollow(UserFollow userFollow);
        Task<List<Follower>> GetFollowers(string userId);
        Task<List<Following>> GetFollowing(string userId);   
    }
}