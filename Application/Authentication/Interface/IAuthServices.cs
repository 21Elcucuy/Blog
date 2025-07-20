using Application.Authentication.DTO;

namespace Application.Authentication.Interface;

public interface IAuthServices
{
    public Task<AuthModel> LoginAsync(LoginRequest loginRequest ,CancellationToken cancellationToken = default);
    public Task<AuthModel> RegisterAsync(RegisterRequest registerRequest , CancellationToken cancellationToken = default ,bool AddAdminCall=false );
    // public Task<string> AddRoleAsync(RoleRequest roleRequest , CancellationToken cancellationToken = default);
    public Task<AddAdminInfo> AddAdminAsync(string Email, CancellationToken cancellationToken = default);
  
    
}