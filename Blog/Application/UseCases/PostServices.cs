using System.Security.Claims;
using Backend.Application.DTOs;

using Backend.Domain.Interfaces;
using Backend.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Backend.Application.UseCases
{
    public class PostServices
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostServices(IPostRepository postRepository , UserManager<ApplicationUser> userManager)
        {
            _postRepository = postRepository;
            _userManager = userManager;
        }
        public async Task<PostDTO> CreatePostAsync(AddPost Addpost)
        {
            if (string.IsNullOrWhiteSpace(Addpost.Title))
            {
                throw new ArgumentException("Title cannot be empty", nameof(Addpost.Title));
            }

          
            
            Post post = new Post
            {
                Title = Addpost.Title,
                SubTitle = Addpost.SubTitle,
                Content = Addpost.Content,
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                AuthorId = Addpost.AuthorId,
            };

            await _postRepository.CreatePostAsync(post);
              
            return new PostDTO
            {
                id = post.id,
                Title = post.Title,
                SubTitle = post.SubTitle,
                Content = post.Content,
                Created = post.Created,
               
            };

        }    
        
    }
}