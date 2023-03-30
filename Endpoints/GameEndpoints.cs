using ApiWeb.Context;
using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Endpoints;
public static class GameEndpoints
{
    public static void MapGameEndpoints(this WebApplication app)
    {
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
    }
}