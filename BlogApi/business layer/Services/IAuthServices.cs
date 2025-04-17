using BlogApi.Helper;
using BlogApi.Models.Domain;
using BlogApi.Models.DTO;
using Microsoft.EntityFrameworkCore.Internal;

namespace BlogApi.businesslayer.Services
{
    public interface IAuthServices
    {
        public Task<AuthModel> RegisterUserAsync(RegisterDTO register);
        public Task<AuthModel> LoginUserAsync(LoginDTO login);
        public Task<string> AddRoleAsync(RoleDTO role);
    }
}