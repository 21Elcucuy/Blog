using Application.AdminServices.DTO;

namespace Application.AdminServices.Interface;

public interface IAdminService
{
    public Task<AdminInfo> TimeOutUser(string UserId, short AmountOfDays);
    public Task<AdminInfo> BanUser(string UserId);
    public Task<AdminInfo> UnbanUser(string UserId);
    
}