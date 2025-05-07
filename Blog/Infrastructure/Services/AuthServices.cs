using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Application.DTOs;
using Backend.Application.Shared.Configurations;
using Backend.Domain.Interfaces;
using Backend.Domain.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Infrastructure.Services
{
    public class AuthServices :IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<JWT> _jwt;
        private readonly RoleManager<IdentityRole> _roleManager;
 

        public AuthServices(UserManager<ApplicationUser> userManager , IOptions<JWT> jwt ,RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._jwt = jwt;
            _roleManager = roleManager;

        }

        public async Task<AuthModel> RegisterUserAsync(RegisterDTO register)
        {
              
                 if(await _userManager.FindByEmailAsync(register.Email) is not null)
                 {
                    return new AuthModel()
                    {
                        IsAuthenticated = false,
                        Message = "Email already exists"
                    };
                 
                 }
                 if(await _userManager.FindByNameAsync(register.Username) is not null)
                 {
                    return new AuthModel()
                    {
                        IsAuthenticated = false,
                        Message = "UserName already exists"
                    };
                 } 
                 var user = new ApplicationUser()
                 {
                     Email = register.Email,
                     UserName = register.Username,
                     FullName = register.FullName,
                     Created = DateTime.UtcNow,
                     Followings = new List<UserFollow>(),
                     Followers = new List<UserFollow>(),
                 };
          

                var Result = await _userManager.CreateAsync(user, register.Password);
                if(!Result.Succeeded)
                {
                    return new AuthModel()
                    {
                        IsAuthenticated = false,
                        Message = "User registration failed"
                    };
                }
                var jwtToken = await GetToken(user);
               return  new AuthModel()
                {
                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    Message = "User registration successful",
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = new List<string> { "User" },
                    ExpiresOn = jwtToken.ValidTo,
                };

    }
    
        public async Task<AuthModel> LoginUserAsync(LoginDTO login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if(user == null && ! await _userManager.CheckPasswordAsync(user,login.Password))
            {
                return new AuthModel()
                {
                    IsAuthenticated = false,
                    Message = "Invalid Email or password"
                };
            }        
                var jwtToken = await GetToken(user);
                var roles = await _userManager.GetRolesAsync(user);
                return new AuthModel()
                {
                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    Message = "User login successful",
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    ExpiresOn = jwtToken.ValidTo,
                };
        }

        public async Task<string> AddRoleAsync(RoleDTO role)
        {
            var user = await _userManager.FindByIdAsync(role.UserId);
            if (user is null || ! await _roleManager.RoleExistsAsync(role.RoleName))
               return "Invalid user ID or Role";
            if(await _userManager.IsInRoleAsync(user,role.RoleName))
               return "Role already exists";
           var Result = await _userManager.AddToRoleAsync(user, role.RoleName);
           if (Result.Succeeded)
           return string.Empty;
           
           return "something went wrong";

        }

        public async Task<JwtSecurityToken> GetToken(ApplicationUser user)
    {
       var Claimuser = await _userManager.GetClaimsAsync(user);
       var roles = await _userManager.GetRolesAsync(user);
       var roleClaim = new List<Claim>();
       foreach (var role in roles)
       {
           roleClaim.Add(new Claim("roles", role));
       }  
       var Claims = new []
       {
         new Claim(JwtRegisteredClaimNames.Sub, user.Id),
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
         new Claim("uid", user.Id)
       }.Union(Claimuser).Union(roleClaim);
       var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
       var SigningCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
       
       var jwtSecurityKey = new JwtSecurityToken(
           issuer: _jwt.Value.Issuer,
           audience: _jwt.Value.Audience,
           claims: Claims,
           expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_jwt.Value.DurationInDays)),
           signingCredentials: SigningCredentials
       );  
       return jwtSecurityKey ;
    }

      
    }
}