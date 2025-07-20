using Application.AdminServices.DTO;
using Application.AdminServices.Interface;
using Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.AdminServices.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(UserManager<ApplicationUser> userManager )
    {
        _userManager = userManager;
    }
    public async  Task<AdminInfo> TimeOutUser(string UserId, short AmountOfDays)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == UserId);
        var Roles = await _userManager.GetRolesAsync(user);
        foreach (var role in Roles)
        {
            if (role == "Admin" || role == "Manager")
                return ReturnAdminInfo(user, false, "The Man is Admin Call The Manager" ,false);
        }

        var Result =  await _userManager.SetLockoutEnabledAsync(user, true);
        if (!Result.Succeeded)
        {
            return ReturnAdminInfo(user, false, "There was an error setting the Lockout" ,false);
        }
        Result =  await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddDays(AmountOfDays));
        if (!Result.Succeeded)
        {
            return ReturnAdminInfo(user, false, "There was an error setting the Lockout" ,false);
        }
        await _userManager.UpdateSecurityStampAsync(user);
        return ReturnAdminInfo(user,true , "Time Out  the User Successfully" ,false);
    }

    public async  Task<AdminInfo> BanUser(string UserId)
    { 
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == UserId);
        var Roles = await _userManager.GetRolesAsync(user);
        foreach (var role in Roles)
        {
            if (role == "Admin" || role == "Manager")
                return ReturnAdminInfo(user, false, "The Man is Admin Call The Manager" ,false);
        }

        var Result =  await _userManager.SetLockoutEnabledAsync(user, true);
        if (!Result.Succeeded)
        {
            return ReturnAdminInfo(user, false, "There was an error setting the Lockout" ,false);
        }
        Result =  await _userManager.SetLockoutEndDateAsync(user,DateTimeOffset.MaxValue);
        if (!Result.Succeeded)
        {
            return ReturnAdminInfo(user, false, "There was an error setting the Lockout" ,false);
        }
        await _userManager.UpdateSecurityStampAsync(user);
        return ReturnAdminInfo(user,true , "You banned forever" ,false );

    }

 
    public async Task<AdminInfo> UnbanUser(string UserId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == UserId);
        if (user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow)
        {
            return ReturnAdminInfo(user, false, "User is not banned", false);
        }   
        var endDateResult  = await _userManager.SetLockoutEndDateAsync(user, null);
        if (!endDateResult.Succeeded)
        {
            return ReturnAdminInfo(user, false, "Error Clearing locout end Date" ,false);
                     
        }
        var DisableResult  = await _userManager.SetLockoutEnabledAsync(user, false);
        if (!DisableResult.Succeeded)
        {
            return ReturnAdminInfo(user,  false, "Error disabling lockout" ,false);
            
        }
      
        return ReturnAdminInfo(user, false , "You unbanned The User" ,true);
    }

    private AdminInfo ReturnAdminInfo(ApplicationUser user, bool BanSuccess,string Message ,bool UnbanSuccess  )
    {
        return new AdminInfo()
        {
            banSuccess = BanSuccess,
            Message = Message,
            UserId = user.Id,
            UserName = user.UserName,
            unbanSuccess = UnbanSuccess,
    
        };
    }
}