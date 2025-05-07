
using Backend.Domain.Models;
using Backend.Infrastructure.Persistence.Database;
using Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Repository
{
    public class SQLPostRepository : IPostRepository
    {
        private readonly BlogContext _context;

       public SQLPostRepository(BlogContext context)
        {
            this._context = context;
        }
        public async  Task<Post> CreatePostAsync(Post post)
        {
        
              await _context.Posts.AddAsync(post);
                
              await _context.SaveChangesAsync();
                return post;
        }

      

        public async Task<Post> DeletePostAsync(Guid id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.id == id);
            if(post == null)
            {
                return null;
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return post;

        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
 
          return await _context.Posts.ToListAsync();
         }

        public Task<Post> GetAtAsync(Guid id)
        {
           var post = _context.Posts.FirstOrDefaultAsync(p => p.id == id);
              return post;

        }

        public async Task<Post> UpdatePostAsync(Guid id, Post post)
        {
            var postToUpdate =await _context.Posts.FirstOrDefaultAsync(p => p.id == id);
            if(postToUpdate == null)
            {
                return null;
            }
            postToUpdate.Title = post.Title;
            postToUpdate.SubTitle = post.SubTitle;
            postToUpdate.Content = post.Content;
            await _context.SaveChangesAsync();
            return postToUpdate;

        }


    }

}