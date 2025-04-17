using BlogApi.DataLayer.Models.Domain;
using BlogApi.Models;
using BlogApi.Models.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Database
{
    
public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Author>()
            .HasOne(a => a.ApplicationUser)
            .WithMany()
            .HasForeignKey(a => a.AuthorId);


            var Role = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = "e267c0b0-6d08-4639-a4fc-53a7595aab4c",
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole()
                {
                    Id = "8a0d4ba5-fa50-419e-954a-e653e692f877",
                    Name = "User",
                    NormalizedName = "User".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            };
            builder.Entity<IdentityRole>().HasData(Role);
            
 
        }
}

}