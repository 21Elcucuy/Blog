using System.Net;
using Application.Common.enums;
using Application.EmailServices.Interface;
using Application.PostSev;
using Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.EmailServices.Services;

public class ConfrimEmailServices : IConfrimEmailServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailServices _emailServices;
    private readonly ILogger<IEmailServices> _logger;

    public ConfrimEmailServices(UserManager<ApplicationUser> userManager , IEmailServices emailServices ,ILogger<IEmailServices> logger)
    {
        _userManager = userManager;
        _emailServices = emailServices;
        _logger = logger;
    }
    
    public async Task<bool> SendConfromationEmailAsync(string  user_id,string Origin,CancellationToken cancellationToken = default) 
    {
        var user = await _userManager.FindByIdAsync(user_id);
        if (user is null || user.EmailConfirmed)
        {
            _logger.LogError(user is null ? "User not found" : "User already confirmed");
            return false;
        }
        var token =  await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmUrl = $"{Origin}/api/Auth/confirmemail?user_id={user.Id}&token={WebUtility.UrlEncode(token)}";
        var emailBody = @$"
Please Confrim Your Email 
<a href=""{confirmUrl}"">Your Url</a>";
        await _emailServices.SendEmailAsync(user.Email,"Confirm Email", emailBody ,cancellationToken : cancellationToken);
        _logger.LogInformation("Email Send Successfully");
        return true;

    }

    
    public async Task<bool> ConfirmEmailAsync(string user_id , string token,CancellationToken cancellationToken = default)
    { 
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == user_id , cancellationToken);
        if (user == null)
        {
            _logger.LogError("User not found");
            return false;
        }
        var result = await _userManager.ConfirmEmailAsync(user,token);
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogInformation("Confirm Email Successfully");
        return result.Succeeded;
    }

  
    // public async Task <SendStatus> ResendEmailAsync(string Email ,string Origin,CancellationToken cancellationToken = default)
    // {
    //     var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == Email ,cancellationToken);
    //     if (user == null)
    //     {
    //         return SendStatus.SendFailed;
    //     }
    //
    //     if (user.EmailConfirmed == true)
    //     {
    //         return SendStatus.AlreadyConfirmed;
    //     }
    //     await SendConfromationEmailAsync(user.Id, Origin,cancellationToken);
    //     return SendStatus.SendSucssfuly;
    //      
    // }

}