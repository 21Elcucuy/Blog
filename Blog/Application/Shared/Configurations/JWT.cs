using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Application.Shared.Configurations
{
    public class JWT
    {
  public string Key { get; set; }
  public string Issuer { get; set; }
  public string Audience { get; set; }

  public double DurationInDays { get; set; }
     
    }
}