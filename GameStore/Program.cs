using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// GET /games
app.MapGet("games", () => games);

// GET /games/id
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName);

// POST /games
app.MapPost("games", (CreateGameDto newGame) =>
{
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
app.MapPut("games/{id}", (int id, UpdateGameDto updateGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    if (index == -1)
    {
        return Results.NotFound( new {Message = "Game Id not found"});
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
app.MapDelete("games/{id}", (int id) =>
    {
        games.RemoveAll(g => g.Id == id);
        
        return Results.NoContent();
    }
);


app.Run();

