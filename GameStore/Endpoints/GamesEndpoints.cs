using GameStore.Dtos;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
        new (
            1,
            "Hollow Knight",
            "Adventure",
            39.99M,
            new DateOnly(2017, 02, 24)),
        new (
            2,
            "Assassin's Creed",
            "Action",
            19.99M,
            new DateOnly(2007, 11, 13)),
        new (
            3,
            "Horizon Zero Dawn",
            "RPG",
            199.99M,
            new DateOnly(2017, 02, 28))

    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/id
        group.MapGet("/{id}", (int id) =>
            {
                
                GameDto? game = games.Find(game => game.Id == id);
        
                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            if (string.IsNullOrEmpty(newGame.Name))
                return Results.BadRequest("Nome é obrigatório!");

            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // PUT /games/id
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound( new {Message = "Id do Jogo não encontrado."});
            }

            games[index] = new GameDto(
                id, 
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );
    
            return Results.NoContent();

        });

        // DELETE /games/id
        group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(g => g.Id == id);
        
                return Results.NoContent();
            }
        );

        return group;
    }
}