
using BlogApi.Models;
using BlogApi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Database
{
    
public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }

}

}