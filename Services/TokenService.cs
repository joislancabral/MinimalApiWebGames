using System.Security.Claims;
using ApiWeb.Models;
using System.Text;

namespace ApiWeb.Services;

public class TokenService : ITokenService
{
    public string GenerateToken(string key, string issuer, string audience, UserModel user)
    {
       var claims = new[]
       {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
       };

       var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }
}