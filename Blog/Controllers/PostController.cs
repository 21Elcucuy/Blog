using System.Security.Claims;
using Application.PostSer.DTO;
using Application.PostSev.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.AuthController;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class PostController : ControllerBase
{
    private readonly IPostServices _postService;

    public PostController(IPostServices postService)
    {
        _postService = postService;
    }
     
    [HttpPost]
    public async Task<IActionResult> CreatePost(AddPostDTO addpost)
    {
        var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var Response = await _postService.CreatePostAsync(addpost , UserId);
         return returnResponse(Response);
    }

    [HttpPut("id")]
    public async Task<IActionResult> UpdatePost(Guid id, UpdatePostDTO updatepost)
    {
        var Response = await _postService.UpdatePostAsync(id, updatepost);
        return returnResponse(Response);
    }

    [HttpDelete("id")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var Response = await _postService.DeletePostAsync(id);
        return returnResponse(Response);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetPost(Guid id)
    {
        var Response = await _postService.GetPostAsync(id);
        return returnResponse(Response);
    }

    [HttpGet]
    [Route("GetAllPost")]
    public async Task<IActionResult> GetAllPost()
    {
        var Response = await _postService.GetAllPostAsync();
        return Ok(Response);
        
    }

    [HttpGet]
    [Route("GetAllPostByUserId")]
    public async Task<IActionResult> GetAllPostByUserId()
    {
        var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var Response = await _postService.GetPostsByUserIdAsync(UserId);
        return Ok(Response);
    }
    private IActionResult returnResponse(PostResponse Response)
    {
        if (!Response.Success)
        {
            return BadRequest(Response.Message);
        }
        return Ok(Response);
    }
}