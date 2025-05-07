using System.Security.Claims;
using Backend.Application.DTOs;
using Backend.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly FollowServicesOutPut _followServicesOutPut;

        public FollowController(FollowServicesOutPut followServicesOutPut)
        {
            _followServicesOutPut = followServicesOutPut;
        }
        [Authorize]
        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] FollowRequset request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserFollowDTO userFollowDTO = new UserFollowDTO()
            {
                 FollowingId = request.UserId,
                 FollowedId = User.FindFirstValue(ClaimTypes.NameIdentifier )
            };
            if (await _followServicesOutPut.Follow(userFollowDTO))
                return Ok(new { message = "Follow Success" });;
            return BadRequest(new {message = "Follow Failed"});
        }
        [Authorize]
        [HttpPost("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] FollowRequset request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserFollowDTO userFollowDTO = new UserFollowDTO()
            {
                FollowingId = request.UserId,
                FollowedId = User.FindFirstValue(ClaimTypes.NameIdentifier )
            };
            if (await _followServicesOutPut.UnFollow(userFollowDTO))
                return Ok(new {message ="Unfollow Success"});
            return BadRequest(new {message = "Unfollow Failed"});
        } 
      [HttpGet("GetFollower/userId")]
      public async Task<IActionResult> GetAllUserFollower([FromQuery] string userId)
      {
          
          return Ok(_followServicesOutPut.GetUserFollowing(userId));
      }
       [HttpGet("GetFollowing/userId")]
      public async Task<IActionResult> GetAllUserFollowing([FromQuery] string userId)
      {
          return Ok(_followServicesOutPut.GetUserFollower(userId));
      }
    }
}