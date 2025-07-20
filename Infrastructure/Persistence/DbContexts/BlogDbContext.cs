using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContexts;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options): base(options)
    {
        
    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Like>()
            .HasKey(l => new { AuthorId = l.UserId, l.PostId });
        
       modelBuilder.Entity<Like>().HasOne(e => e.Post)
           .WithMany(p => p.Likes)
           .HasForeignKey(e => e.PostId)
           .OnDelete(DeleteBehavior.Cascade);
       
    
    }
}