using Backend.Application.DTOs;
using Backend.Domain.Interfaces;
using Blog.Infrastructure;

namespace Backend.Application.UseCases;

public class FollowServicesOutPut
{
    private readonly IFollowServices _followServices;

    public FollowServicesOutPut(IFollowServices followServices)
    {
        _followServices = followServices;
    }

    public async Task<bool> Follow(UserFollowDTO userFollow)
    {
        FollowDTO followDto = new FollowDTO()
        {
            FollowedId = userFollow.FollowedId,
            FollowingId = userFollow.FollowingId,

        };
        return await _followServices.Follow(followDto);
  
    }
    public async Task<bool> UnFollow(UserFollowDTO userFollow)
    {
        FollowDTO followDto = new FollowDTO()
        {
            FollowedId = userFollow.FollowedId,
            FollowingId = userFollow.FollowingId,

        };
        return await _followServices.Unfollow(followDto);
   
    }

    public async Task<List<GetAllFollowOutPut>> GetUserFollower(string userId)
    {
        List<GetAllFollowOutPut> userFollows = new List<GetAllFollowOutPut>();
          
         var Users = await _followServices.GetAllFollower(userId);
         if (Users == null)
             return null;
         foreach (var User in Users)
         {
             userFollows.Add(new GetAllFollowOutPut()
             {
                 UserName = User.UserName,
                 Email = User.Email,
             });
         }

         ;
         return userFollows;





    }

    public async Task<List<GetAllFollowOutPut>> GetUserFollowing(string userId)
    {
        List<GetAllFollowOutPut> userFollows = new List<GetAllFollowOutPut>();
        var Users = await _followServices.GetAllFollowing(userId);
        if(Users == null)
            return null;
        
        foreach (var User in Users)
        {
            userFollows.Add(new GetAllFollowOutPut()
            {
                UserName = User.UserName,
                Email = User.Email,
            });
        };
        
        return userFollows;
    }
}