using Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Seed;

public static class IdentitySeed
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
    
        string[] roleNames = { "Admin", "User" ,"Manager" };
        foreach (var role in roleNames)
        {
         if(!await roleManager.RoleExistsAsync(role)) 
             await roleManager.CreateAsync(new IdentityRole(role));
             
        }
    }

    public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
    {
        

    }
}