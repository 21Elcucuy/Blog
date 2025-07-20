using System.Net;
using Application.EmailServices.Interface;
using Application.PostSev;
using Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.EmailServices.Services;

public class ResetPasswordSevices :IResetPasswordServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailServices _emailServices;

    public ResetPasswordSevices(UserManager<ApplicationUser> userManager , IEmailServices  emailServices)
    {
        _userManager = userManager;
        _emailServices = emailServices;
    }

    public async Task<bool> SendResetPasswordAsync(string Email , string Origin ,CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == Email , cancellationToken );
        if (user == null)
        {
            return false;
        }
        var Token = await _userManager.GeneratePasswordResetTokenAsync(user);
        cancellationToken.ThrowIfCancellationRequested();
        var ResetPassword = $"http://localhost:5173/resetpassword?user_id={user.Id}&token={WebUtility.UrlEncode(Token)}";
        var EmailBody = @$"Reset Your Password Here 
         <a href=""{ResetPassword}"">Reset Password</a>"; 
        await _emailServices.SendEmailAsync(user.Email, "Reset your Password" , EmailBody  , cancellationToken: cancellationToken);
       return true;
    }

    public async Task<bool> ResetPasswordAsync(string user_id , string token ,string newPassword, CancellationToken cancellationToken = default)
    {
         var user =  await _userManager.Users.FirstOrDefaultAsync(x => x.Id == user_id , cancellationToken);
         var Result = await _userManager.ResetPasswordAsync(user, token, newPassword);
         return Result.Succeeded;
    }

   
}