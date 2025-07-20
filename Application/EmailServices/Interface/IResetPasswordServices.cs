using Domain.Entity.Identity;

namespace Application.EmailServices.Interface;

public interface IResetPasswordServices
{
    public Task<bool> SendResetPasswordAsync(string  Email, string Origin,
        CancellationToken cancellationToken = default);
    public Task<bool> ResetPasswordAsync(string user_id , string token ,string newPassword, CancellationToken cancellationToken = default);
  
}