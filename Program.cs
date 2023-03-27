using ApiWeb.Context;
using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? MySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options
.UseMySql(MySqlConnection, ServerVersion.AutoDetect(MySqlConnection)));

var app = builder.Build();

//Classification
app.MapGet("/classifications",  async (AppDbContext contextDb) =>
{
    return await contextDb.Classifications.ToListAsync();
});

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

app.Run();
