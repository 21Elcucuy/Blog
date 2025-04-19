using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BlogApi.DataLayer.Models.Domain;
using BlogApi.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.BusinessLayer.Repository
{
    public class FollowRepository:IFollowRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public FollowRepository(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<bool> Follow(UserFollow userFollow)
        {
              if(!await AddFollowing(userFollow))
               {
                return false;
                }
            UserFollow userFollow2 = new UserFollow()
            {
                UserId = userFollow.FollowId,
                FollowId = userFollow.UserId,
            };
            if(!await AddFollower(userFollow2))
              return false;
            return true;
        }
          public async Task<bool> Unfollow(UserFollow userFollow)
        {
             if(!await RemoveFollowing(userFollow))
             {
                return false;
             }
            UserFollow userFollow2 = new UserFollow()
            {
                UserId = userFollow.FollowId,
                FollowId = userFollow.UserId,
            };
            if(!await RemoveFollower(userFollow2))
            {
                return false;
            }
            return true;
        }
        private async Task<bool> AddFollower(UserFollow userFollow)
        {  
            var user = await _userManager.Users.Include(x => x.Followers).FirstOrDefaultAsync(x => x.Id == userFollow.UserId);             
           
            if(user is null)
            {
                return false;
            }     
                var follower = new Follower()
                {
                    FollowerId = userFollow.FollowId,
                };
                user.Followers.Add(follower);
                await _userManager.UpdateAsync(user);
                return true;
        }

        private async Task<bool> AddFollowing(UserFollow userFollow)
        {
            var user = await _userManager.Users.Include(x => x.Followings).FirstOrDefaultAsync(x => x.Id == userFollow.UserId);   
           
            if(user is null)
            {
                return false;
            }

            var following = new Following()
            {
                FollowingId = userFollow.FollowId,
            };
            user.Followings.Add(following);
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<List<Follower>> GetFollowers(string userId)
        {
              var user = await _userManager.Users.Include(x => x.Followers).Include(x => x.Followings).FirstOrDefaultAsync(x => x.Id == userId);
            if(user is null || user.Followers is null)
            {
                return new List<Follower>();
            }
            
            return user.Followers.ToList();
        }

        public async Task<List<Following>> GetFollowing(string userId)
        {
             var user = await _userManager.Users.Include(x => x.Followers).Include(x => x.Followings).FirstOrDefaultAsync(x => x.Id == userId);
            if(user is null || user.Followings is null)
            {
                return new List<Following>();
            }
            
            return user.Followings.ToList();
        }

        private async Task<bool> RemoveFollower(UserFollow userFollow)
        {
            var user = CheckUsers(userFollow).Result;
            if(user is null)
            {
                return false;
            }
            var follower = user.Followers.FirstOrDefault(x => x.FollowerId == userFollow.FollowId);

            if(follower is null)
            {
                return false;
            }

            user.Followers.Remove(follower);
            await _userManager.UpdateAsync(user);
            return true;
        }
        private async Task<bool> RemoveFollowing(UserFollow userFollow)
        {
            var user = CheckUsers(userFollow).Result;
            if(user is null)
            {
                return false;
            }
            var following = user.Followings.FirstOrDefault(x => x.FollowingId == userFollow.UserId);

            if(following is null)
            {
                return false;
            }

            user.Followings.Remove(following);
            await _userManager.UpdateAsync(user);
            return true;
        }

     

        private async  Task<ApplicationUser> CheckUsers(UserFollow userFollow)
        {
                var user = await _userManager.Users.Include(x => x.Followers).Include(x => x.Followings).FirstOrDefaultAsync(x => x.Id == userFollow.UserId);
                 if(user is null || await _userManager.FindByIdAsync(userFollow.FollowId) is null)
                 {
                     return null;
                 }
                 return user;
        }
      
    }
}