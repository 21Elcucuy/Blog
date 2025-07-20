using Domain.Entity;
using Domain.Interface;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class SqlPostRepository : IPostRepository
{
    private readonly BlogDbContext _context;

    public SqlPostRepository(BlogDbContext context)
    {
        _context = context;
    }
    public async Task<Post> CreatePostAsync(Post post)
    {
         await _context.Posts.AddAsync(post);
         await _context.SaveChangesAsync();
        return post; 
    }

    public async Task<Post> UpdatePostAsync(Guid postid,Post post)
    {
       var UpdatePost =await  _context.Posts.FirstOrDefaultAsync(p => p.Id == postid);
       if (UpdatePost == null)
       {
           return null;
           
       }
       UpdatePost.Title = post.Title;
       UpdatePost.Content = post.Content;
       
       await _context.SaveChangesAsync();
        return post; 
    }
    

    public async Task<Post> DeletePostAsync(Guid postid)
    {
       var DeletePost =_context.Posts.FirstOrDefault(p => p.Id == postid);
       if (DeletePost == null)
       {
           return null;
       }
       _context.Posts.Remove(DeletePost);
       await _context.SaveChangesAsync();
       return DeletePost;
    }

    public async Task<Post> GetPostAsync(Guid postId)
    { 
        return await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<List<Post>> GetAllPostAsync()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<List<Post>> GetPostsByUserIdAsync(string userId)
    {
        var Posts =  _context.Posts.Where(p => p.AuthorId == userId).ToList();
        return Posts;
        
    }
}