
using Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Database
{
    
public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }

}

}