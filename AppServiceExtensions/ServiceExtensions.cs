using System.Text;
using ApiWeb.Context;
using ApiWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiWeb.AppServiceExtensions;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddConnection(this WebApplicationBuilder builder)
    {
        string? MySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AppDbContext>(options => options
        .UseMySql(MySqlConnection, ServerVersion.AutoDetect(MySqlConnection)));

        //Service Authentication Token
        builder.Services.AddSingleton<ITokenService>(new TokenService());
        return builder;
    }

    public static WebApplicationBuilder AddAutorizationEx(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        builder.Services.AddAuthorization();
        return builder;
    }
}