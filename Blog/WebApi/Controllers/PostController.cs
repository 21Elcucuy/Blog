using System.Security.Claims;
using Backend.Application.DTOs;
using Backend.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostServices _postOutput;

        public PostController(PostServices postOutput)
        {
            _postOutput = postOutput;
        }
         
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] AddPostDTO addPostDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if (addPostDTO == null)
            {
                return BadRequest("Post data is null.");
            }

            AddPost AddPost = new AddPost()
            {
                Title = addPostDTO.Title,
                Content = addPostDTO.Content,
                SubTitle = addPostDTO.SubTitle,
                AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            try
            {
                var result = await _postOutput.CreatePostAsync(AddPost);
                return CreatedAtAction(nameof(CreatePost), new { id = result.id }, result);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        } 
     
    }
}