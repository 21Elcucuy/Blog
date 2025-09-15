using System.IdentityModel.Tokens.Jwt;
using Domain.Entity.Identity;

namespace Application.Authentication.Interface;

public interface ITokenProvider
{
    Task<JwtSecurityToken> GetToken(ApplicationUser user, CancellationToken cancellationToken =default);
}