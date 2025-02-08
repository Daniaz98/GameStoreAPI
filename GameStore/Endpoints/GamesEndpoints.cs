using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToDto())
                    .AsNoTracking()
                    .ToListAsync());

        // GET /games/id
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                
                Game? game = await dbContext.Games.FindAsync(id);
        
                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
            })
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);
    

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToDto());
        });

        // PUT /games/id
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound( new {Message = "Id do Jogo não encontrado."});
            }
                
            dbContext.Entry(existingGame).CurrentValues.SetValues(updateGame.ToEntity(id));
            
            await dbContext.SaveChangesAsync();
    
            return Results.NoContent();

        });

        // DELETE /games/id
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
        
                return Results.NoContent();
            }
        );

        return group;
    }
}