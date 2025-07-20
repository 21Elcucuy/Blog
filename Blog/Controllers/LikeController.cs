using System.Security.Claims;
using Application.LikeServices.DTO;
using Application.LikeServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }
        [HttpPost("like")]
        public async Task<IActionResult> LikePost(AddLikeDTO likeDto)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response =  await _likeService.AddLikeAsync(likeDto, user);
            if (response is null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(response);
        }

        [HttpDelete("Unlike")]
        public async Task<IActionResult> UnlikePost(DeleteLikeDTO likeDto)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _likeService.DeleteLikeAsync(likeDto, user);
            if (response is null)
            {
                return BadRequest("Something went wrong");
            }    
            return Ok(response);
        }

        [HttpGet("GetAllUserLikes")]
        public async Task<IActionResult> GetAllUserLikes()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response =await _likeService.GetAllLikesForUserAsync(user);
            return Ok(response);
        }

        [HttpGet("GetAllPostLikes")]
        public async Task<IActionResult> GetAllPostLikes([FromQuery(Name = "PostId")] Guid PostId)
        {
           var  response = await _likeService.GetAllLikesForPostAsync(PostId);
            return Ok(response);
        }
    }
}
