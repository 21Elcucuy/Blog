

using Backend.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Database    
{
    
public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {

    }

    public DbSet<UserFollow> UserFollows { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
                                
            builder.Entity<UserFollow>().HasKey(p => new { p.FollowingId, p.FollowedId });
           
            builder.Entity<UserFollow>().HasOne(p => p.Follower).WithMany(f => f.Followers )
                .HasForeignKey(p => p.FollowedId).OnDelete(DeleteBehavior.Restrict);
           
            builder.Entity<UserFollow>().HasOne(p => p.Following).WithMany(f => f.Followings )
                .HasForeignKey(p => p.FollowingId).OnDelete(DeleteBehavior.Restrict);
            

       

             
              
            // var Role = new List<IdentityRole>()
            // {
            //     new IdentityRole()
            //     {
            //         Id = "e267c0b0-6d08-4639-a4fc-53a7595aab4c",
            //         Name = "Admin",
            //         NormalizedName = "Admin".ToUpper(),
            //         ConcurrencyStamp = Guid.NewGuid().ToString()
            //     },
            //     new IdentityRole()
            //     {
            //         Id = "8a0d4ba5-fa50-419e-954a-e653e692f877",
            //         Name = "User",
            //         NormalizedName = "User".ToUpper(),
            //         ConcurrencyStamp = Guid.NewGuid().ToString()
            //     }
            // };
            // builder.Entity<IdentityRole>().HasData(Role);
            
 
        }
}

}