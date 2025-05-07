using Backend.Application.DTOs;
using Microsoft.EntityFrameworkCore.Internal;

namespace Backend.Domain.Interfaces
{
    public interface IAuthServices
    {
        public Task<AuthModel> RegisterUserAsync(RegisterDTO register);
        public Task<AuthModel> LoginUserAsync(LoginDTO login);
        public Task<string> AddRoleAsync(RoleDTO role);
    }
}