using ApiWeb.Models;
using ApiWeb.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiWeb.Endpoints;
public static class AuthenticationEndPoints
{
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if (userModel == null)
            {
                return Results.BadRequest("Login invalided");
            }
            if (userModel.UserName == "fariasgames" && userModel.Password == "better123")
            {
                var tokenString = tokenService.GenerateToken(app.Configuration["Jwt:Key"],
                    app.Configuration["Jwt:Issuer"],
                    app.Configuration["Jwt:Audience"],
                    userModel);
                return Results.Ok(new { token = tokenString });
            }
            else
            {
                return Results.BadRequest("Login invalided");
            }

        }).Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("Login")
            .WithTags("Authentication");
    }
}