using System.Text;
using ApiWeb.Context;
using ApiWeb.Models;
using ApiWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

string? MySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options
.UseMySql(MySqlConnection, ServerVersion.AutoDetect(MySqlConnection)));

//Service Authentication Token
builder.Services.AddSingleton<ITokenService>(new TokenService());

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

var app = builder.Build();

//Endpoint login
app.MapPost("/login",[AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
{
    if (userModel == null)
    {
        return Results.BadRequest("Login invalided");
    }
    if(userModel.UserName == "fariasgames" && userModel.Password == "better123")
    {
        var tokenString = tokenService.GenerateToken(app.Configuration["Jwt:Key"],
            app.Configuration["Jwt:Issuer"],
            app.Configuration["Jwt:Audience"],
            userModel);
            return Results.Ok(new {token = tokenString});
    }
    else
    {
        return Results.BadRequest("Login invalided");
    }

}).Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status200OK)
    .WithName("Login")
    .WithTags("Authentication");

//Classification
app.MapGet("/classifications",  async (AppDbContext contextDb) =>
{
    return await contextDb.Classifications.ToListAsync();
}).RequireAuthorization();

app.MapGet("/classifications/{id:int}", async (int id, AppDbContext dbContext) => 
{
    return await dbContext.Classifications.FindAsync(id) is Classification classification ? Results.Ok(classification) : Results.NotFound();
});

app.MapPost("/classifications", async (Classification classification, AppDbContext contextDb) =>
{
    contextDb.Classifications.Add(classification);
    await contextDb.SaveChangesAsync();

    return Results.Created($"/classifications/{classification.ClassificationId}", classification);
});

app.MapPut("/classifications/{id:int}", async (int id, AppDbContext dbContext, Classification classification) =>
{
    if (id != classification.ClassificationId)
    {
        return Results.BadRequest();

    } 

    var classificationsDb = await dbContext.Classifications.FindAsync(id);
   
    if(classificationsDb is null)
    {
        return Results.NotFound();
    } 
    
    classificationsDb.Type = classification.Type;
    classificationsDb.AgeRating = classification.AgeRating;

    await dbContext.SaveChangesAsync();
    return Results.Ok(classificationsDb);
});

app.MapDelete("/classifications/{id:int}", async (int id, AppDbContext dbContext) =>
{
    var classificationsDb =  await dbContext.Classifications.FindAsync(id);
    if(classificationsDb is null)
    {
        return Results.NotFound();
    }

    dbContext.Classifications.Remove(classificationsDb);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

//Game
app.MapGet("/games",  async (AppDbContext contextDb) =>
{
    return await contextDb.Games.ToListAsync();
});

app.MapGet("/games/{id:int}", async (int id, AppDbContext dbContext) => 
{
    return await dbContext.Games.FindAsync(id) is Game game ? Results.Ok(game) : Results.NotFound();
});

app.MapPost("/games", async (Game game, AppDbContext contextDb) =>
{
    contextDb.Games.Add(game);
    await contextDb.SaveChangesAsync();

    return Results.Created($"/games/{game.GameId}", game);
});

app.MapPut("/games/{id:int}", async (int id, AppDbContext dbContext, Game game) =>
{
    if (id != game.GameId)
    {
        return Results.BadRequest();

    } 

    var gamesDb = await dbContext.Games.FindAsync(id);
   
    if(gamesDb is null)
    {
        return Results.NotFound();
    } 
    
    gamesDb.Name = game.Name;
    gamesDb.Quantity = game.Quantity;
    gamesDb.Price = game.Price;

    await dbContext.SaveChangesAsync();
    return Results.Ok(gamesDb);
});

app.MapDelete("/games/{id:int}", async (int id, AppDbContext dbContext) =>
{
    var gamesDb =  await dbContext.Games.FindAsync(id);
    if(gamesDb is null)
    {
        return Results.NotFound();
    }

    dbContext.Games.Remove(gamesDb);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();
