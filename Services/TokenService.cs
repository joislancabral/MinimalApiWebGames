using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiWeb.Models;
using Microsoft.IdentityModel.Tokens;

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

       var crendentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

       var token = new JwtSecurityToken(issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.Now.AddMinutes(10),
        signingCredentials: crendentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
}