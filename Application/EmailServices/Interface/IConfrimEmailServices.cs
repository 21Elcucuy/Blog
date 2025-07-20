using Application.Common.enums;

namespace Application.EmailServices.Interface;

public interface IConfrimEmailServices
{
    public  Task<bool> SendConfromationEmailAsync(string user_id, string Origin, CancellationToken cancellationToken = default);
    public Task<bool> ConfirmEmailAsync(string user_id, string token, CancellationToken cancellationToken = default);

    
}