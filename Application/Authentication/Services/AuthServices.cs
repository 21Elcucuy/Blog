using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Authentication.DTO;
using Application.Authentication.Interface;
using Application.EmailServices.Interface;
using Application.PostSev;

using Domain.Entity;
using Domain.Entity.Identity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.JWT;

namespace Application.Authentication.Services;

public class AuthServices : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IConfrimEmailServices _confrimemail;

    public AuthServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager , ITokenProvider tokenProvider ,IConfrimEmailServices Confrimemail)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenProvider = tokenProvider;
        _confrimemail = Confrimemail;
    }
    public async Task<AuthModel> LoginAsync(LoginRequest loginRequest , CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        cancellationToken.ThrowIfCancellationRequested();
        if(user == null || ! await _userManager.CheckPasswordAsync(user,loginRequest.Password) || !user.EmailConfirmed)
        {
            return new AuthModel()
            {
                IsAuthenticated = false,
                Message = "Password or Email in Incorrect"
            };
        }

        if (await _userManager.GetLockoutEnabledAsync(user)  && user.LockoutEnd.HasValue)
        {
            return new AuthModel()
            {
                IsAuthenticated = false,
                Message = "Lockout Wait Until " + user.LockoutEnd.Value.UtcDateTime.ToString("u"),
            };
        }
        var jwtToken = await  _tokenProvider.GetToken(user);
        var roles = await _userManager.GetRolesAsync(user);
        cancellationToken.ThrowIfCancellationRequested();
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

    public async Task<AuthModel> RegisterAsync(RegisterRequest registerRequest ,CancellationToken cancellationToken = default ,bool AddAdminCall = false)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await _userManager.FindByEmailAsync(registerRequest.Email);
        cancellationToken.ThrowIfCancellationRequested();   
        if (user != null)
        {
            
            if (user.EmailConfirmed == false)
            {
                var Token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                cancellationToken.ThrowIfCancellationRequested();
                await _confrimemail.SendConfromationEmailAsync(user.Id,Token ,cancellationToken);
                return new AuthModel() {Message ="Please confirm your account by Chicking on Gmail" , IsAuthenticated = false};
            }
            return new AuthModel() { Message = "Email Already Rgisterd", IsAuthenticated = false };

        }

        if (await _userManager.FindByNameAsync(registerRequest.UserName) is not null)
        {
            return new AuthModel() { Message = "UserName Already Used", IsAuthenticated = false };
        }

        var User = new ApplicationUser()
        {
            Email = registerRequest.Email,
            UserName = registerRequest.UserName,
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            
        };
        var token= await _userManager.GenerateEmailConfirmationTokenAsync(User);
        cancellationToken.ThrowIfCancellationRequested();
        var Result = await _userManager.CreateAsync(User, registerRequest.Password );
        cancellationToken.ThrowIfCancellationRequested();
        if (!Result.Succeeded)
        {
            return new AuthModel() { Message = "Somthing went Wrong", IsAuthenticated = false };
        }
        var AddToUserRole = await _userManager.AddToRoleAsync(User, "User");
        cancellationToken.ThrowIfCancellationRequested();
        if (!AddToUserRole.Succeeded)
        {
            return new AuthModel() { Message = "Somthing went Wrong", IsAuthenticated = false };
        }
        await _confrimemail.SendConfromationEmailAsync(User.Id ,"http://localhost:5140" , cancellationToken);
        var jwtToken = await _tokenProvider.GetToken(User , cancellationToken);
        return new AuthModel() 
            { 
                Message = "Please Check Email", 
                IsAuthenticated = true ,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Username = User.UserName,
                Email = User.Email,
                Roles = new List<string>{"User"},
                ExpiresOn = jwtToken.ValidTo,
            };
        
    }

    public async Task<AddAdminInfo> AddAdminAsync(string Email,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == Email ,cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        
        if (user == null)
        {
            return new AddAdminInfo() { IsAuthenticated = false, Message = "Please Make Account First"};
        }

        RoleRequest roleRequest = new RoleRequest() { UserId = user.Id, RoleName = "Admin" };
        var Result = await AddRoleAsync(roleRequest, cancellationToken);
        if (!string.IsNullOrEmpty(Result))
        {
            return new AddAdminInfo() { Message = "Somthing went Wrong", IsAuthenticated = false };
        }

        return new AddAdminInfo()
        {
            IsAuthenticated = true,
            Email = user.Email,
            Message = "Successfully Added Admin Confrim His email",
            Username = user.UserName,
          
        };

    }


    public async Task<string> AddRoleAsync(RoleRequest roleRequest ,CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var user = await _userManager.FindByIdAsync(roleRequest.UserId);
         cancellationToken.ThrowIfCancellationRequested();
        if (user is null || ! await _roleManager.RoleExistsAsync(roleRequest.RoleName))
            return "Invalid user ID or Role";
        if(await _userManager.IsInRoleAsync(user,roleRequest.RoleName))
            return "Role already exists";
        
        var Result = await _userManager.AddToRoleAsync(user, roleRequest.RoleName);
        cancellationToken.ThrowIfCancellationRequested();
        if (Result.Succeeded)
            return string.Empty;
           
        return "something went wrong";
    }

 
}