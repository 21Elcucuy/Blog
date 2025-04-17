using BlogApi.Models;
using BlogApi.Models.Domain;
using BlogApi.Models.DTO;
using BlogApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            this._postRepository = postRepository;
        }
      
          [HttpGet]
          public async Task<IActionResult> GetAll()
          {
             List<Post> postDomain = await _postRepository.GetAllPostsAsync();
              List<PostDTO> postDTO = new List<PostDTO>();
              foreach (var post in postDomain)
              {
                    PostDTO postdto = new PostDTO()
                    {
                        id = post.id,
                        Title = post.Title,
                        SubTitle = post.subTitle,
                        Content = post.Content,
                        Created = post.Created
                    };
                    postDTO.Add(postdto);
              }

               return Ok(postDTO);

          } 

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPostDTO postDTO)
        {
            Post postDomain = new Post()
            {
                Title = postDTO.Title,
                subTitle = postDTO.SubTitle,
                Content = postDTO.Content,
                Created = DateTime.Now
            };
            await _postRepository.CreatePostAsync(postDomain);
            return Ok(postDomain);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAt(Guid id)
        {
            var post = await _postRepository.GetAtAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            PostDTO postDTO = new PostDTO()
            {
                id = post.id,
                Title = post.Title,
                SubTitle = post.subTitle,
                Content = post.Content,
                Created = post.Created
            };
            return Ok(postDTO);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDTO postDTO)
        {
             var post = new Post()
             {
                    Title = postDTO.Title,
                    subTitle = postDTO.SubTitle,
                    Content = postDTO.Content,
             };
            var updatedPost = await _postRepository.UpdatePostAsync(id, post);
            var newPostDTO = new PostDTO()
            {
                id = updatedPost.id,
                Title = updatedPost.Title,
                SubTitle = updatedPost.subTitle,
                Content = updatedPost.Content,
                Created = updatedPost.Created
            };
            return Ok(newPostDTO);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _postRepository.DeletePostAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
    }
}