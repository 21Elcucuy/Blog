using System.Security.Principal;
using BlogApi.BusinessLayer.Repository;
using BlogApi.DataLayer.Models.Domain;
using BlogApi.DataLayer.Models.DTO;
using BlogApi.Models.Domain;
using BlogApi.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowingController : ControllerBase
    {
        private readonly IFollowRepository _followRepository;

        public FollowingController(IFollowRepository followRepository)
        {
            _followRepository = followRepository;
        }
        
        [HttpGet] 
        [Route("follower")]
        public async Task<IActionResult> GetAllFollower([FromQuery]AddFollowDTO addFollowerDTO)
        {
            Follower follow = new Follower()
            {
                FollowerId = addFollowerDTO.UserId,
            };
            var user = _followRepository.GetFollowers(follow.FollowerId);
            if (user is null)
            {
                return NotFound("User not found");
            }

            
            var followerDTO = new List<FollowerDTO>();
            foreach (var follower in user.Result)
            {
                followerDTO.Add(new FollowerDTO()
                {
                    Follower = follower,
                });
            }
                return Ok(followerDTO);
        }
            
      [HttpGet] 
      [Route("following")]
        public async Task<IActionResult> GetAllFollowing([FromQuery]AddFollowDTO addFollowerDTO)
        {
            Following follow = new Following()
            {
                FollowingId = addFollowerDTO.UserId,
            };
            var user = _followRepository.GetFollowing(follow.FollowingId);
            if (user is null)
            {
                return NotFound("User not found");
            }
            var follownigDTO = new List<FollowingDTO>();
            foreach (var following in user.Result)
            {
                follownigDTO.Add(new FollowingDTO()
                {
                    Following = following,
                });
            }
                return Ok(follownigDTO);
        }
        [HttpPut]
        [Route("{UserId}/{FollowId}")]
        public async Task<IActionResult> Follow(string UserId , string FollowId)
        {
            UserFollow userFollow = new UserFollow()
            {
                UserId = UserId,
                FollowId = FollowId,
            };
           if( await _followRepository.Follow(userFollow))
            {
                return Ok("Followed successfully");
            }
            else
            {
                return BadRequest("Failed to follow user");
            }
            
        }
        [HttpDelete]
        public async Task<IActionResult> Unfollow([FromBody]FollowDTO followDTO)
        {
            UserFollow userFollow = new UserFollow()
            {
                UserId = followDTO.UserId,
                FollowId = followDTO.FollowingId,
            };
            if (await _followRepository.Unfollow(userFollow))
            {
                return Ok("Unfollowed successfully");
            }
            else
            {
                return BadRequest("Failed to unfollow user");
            }
    }
    }
}