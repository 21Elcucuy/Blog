using Application.LikeServices.DTO;
using Application.LikeServices.Interface;
using Domain.Entity;
using Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.LikeServices.Extinstion;

public class LikeMapping  : ILikeMapping
{
    private readonly UserManager<ApplicationUser> _userManager;

    public LikeMapping(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public Like ToLike(AddLikeDTO addlikeDTO, string UserId)
    {
        return new Like()
        {
            UserId = UserId,
            PostId = addlikeDTO.PostId
        };
    }

    public Like ToLike(DeleteLikeDTO addlikeDTO, string UserId)
    {
        return new Like()
        {
            UserId = UserId,
            PostId = addlikeDTO.PostId
        };
    }

    public async Task<LikeResponse> ToLikeResponse(Like like, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == like.UserId,cancellationToken);
        if (user is null)
        {
            return null;
        }

        return new LikeResponse()
        {
            PostId = like.PostId,
            UserName = user.UserName,
        };


    }

    public async  Task<List<LikeResponse>> ToListOfLikeResponse(List<Like> likes, CancellationToken cancellationToken = default)
    {
      var UsersId = likes.Select(x => x.UserId).Distinct().ToList();
      var users = await  _userManager.Users.Where(x => UsersId.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x.UserName ,cancellationToken);
      var response = new List<LikeResponse>();
      foreach (var like in likes)
      {
          response.Add( new LikeResponse()
          {
              PostId = like.PostId,
              UserName = users.TryGetValue(like?.UserId, out var user) ? user : null,
          });
      }
      return response;

    }
}