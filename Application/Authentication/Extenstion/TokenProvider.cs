using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Authentication.Interface;
using Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.JWT;

namespace Application.Authentication.Extenstion;

public class TokenProvider : ITokenProvider
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOptions<JWT> _jwt;

    public TokenProvider(UserManager<ApplicationUser> userManager ,IOptions<JWT> jwt)
    {
        _userManager = userManager;
        _jwt = jwt;
    }
    public async  Task<JwtSecurityToken> GetToken(ApplicationUser user , CancellationToken cancellationToken = default)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        cancellationToken.ThrowIfCancellationRequested();
        var roles = await _userManager.GetRolesAsync(user);
        cancellationToken.ThrowIfCancellationRequested();
        var roleClaim = new List<Claim>();
        foreach (var role in roles)
        {
            roleClaim.Add(new Claim("roles", role));
            
        }
        var SecurityStamp = await _userManager.GetSecurityStampAsync(user);
        cancellationToken.ThrowIfCancellationRequested();
         
         
        var Claim = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
            new Claim("uid", user.Id),
            new Claim("AspNet" +
                      ".Identity.SecurityStamp", SecurityStamp),
        }.Union(roleClaim).Union(userClaims);
        
        var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.SecretKey));
        var SigningCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityKey = new JwtSecurityToken(
            issuer: _jwt.Value.Issuer,
            audience: _jwt.Value.Audience,
            claims: Claim,
            expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_jwt.Value.DurationInDays)),
            signingCredentials: SigningCredentials
        );
        return jwtSecurityKey;
    }
}