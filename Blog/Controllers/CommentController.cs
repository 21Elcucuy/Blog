using System.Security.Claims;
using Application.CommentServices.DTO;
using Application.CommentServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(AddCommentDTO commentDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var commentResponse = await _commentService.AddComment(commentDto, userId);
            if (commentResponse == null)
            {
                return BadRequest();
            }
            return Ok(commentResponse);
        }

        [HttpGet("GetAllCommentsForPost")]
        public async Task<IActionResult> GetAllCommentsForPost([FromQuery(Name ="PostId")]Guid PostId)
        {
            var commentResponse = await _commentService.GetAllCommentsForPost(PostId);
            return Ok(commentResponse);
        }

        [HttpGet("GetAllCommentsForUser")]
        public async Task<IActionResult> GetAllCommentsForUser()
        {
            var  userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var commentResponse = await _commentService.GetAllCommentForUser(userId);
            return Ok(commentResponse);
        }

        [HttpPut("UpdateComment")]
        public async Task<IActionResult> UpdateComment([FromBody]UpdateCommentDTO commentDto)
        {
            var  userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var CommentResponse = await _commentService.UpdateComment(commentDto ,userId);
            if (CommentResponse == null)
            {
                return BadRequest("Something went wrong");
                    
            }
            return Ok(CommentResponse);
            
        }

        [HttpDelete("DeleteComment")]
        public async Task<IActionResult> DeleteComment([FromQuery(Name ="CommentId")]Guid CommentId)
        {
            var  userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var CommentResponse = await _commentService.DeleteComment(CommentId ,userId);
            if (CommentResponse == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(CommentResponse);
        }

        [HttpGet("GetComment")]
        public async Task<IActionResult> GetComment([FromQuery(Name ="CommentId")] Guid CommentId)
        {
            var CommentResponse = await _commentService.GetComment(CommentId);
            if (CommentResponse == null)
            {
                return BadRequest("Something went wrong");
            }
            return  Ok(CommentResponse);
        }
    }
}
