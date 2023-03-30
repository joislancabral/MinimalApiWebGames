using ApiWeb.Context;
using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Endpoints;
public static class ClassificationEndpoints
{
    public static void MapClassificationEndpoints(this WebApplication app)
    {
        app.MapGet("/classifications", async (AppDbContext contextDb) =>
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

            if (classificationsDb is null)
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
            var classificationsDb = await dbContext.Classifications.FindAsync(id);
            if (classificationsDb is null)
            {
                return Results.NotFound();
            }

            dbContext.Classifications.Remove(classificationsDb);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}