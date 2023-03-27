using ApiWeb.Models;

namespace ApiWeb.Services;

public interface ITokenService
{
    string GenerateToken(string key, string issuer, string audience, UserModel user);
}