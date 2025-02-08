using GameStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
   public DbSet<Game> Games => Set<Game>();

   public DbSet<Genre> Genres => Set<Genre>();

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       
       
      modelBuilder.Entity<Genre>().HasData(
            new {Id = 1, Name = "Action"},
            new {Id = 2, Name = "Adventure"},
            new {Id = 3, Name = "Horror"},
            new {Id = 4, Name = "Sports"},
            new {Id = 5, Name = "Racing"},
            new {Id = 6, Name = "Kids and Family"}
         );

         modelBuilder.Entity<Game>().HasData(
             new Game
             {
                 Id = 1,
                 Name = "Hollow Knight",
                 Price = 39.99M,
                 ReleaseDate = new DateOnly(2017, 02, 24),
                 GenreId = 2 // Adventure
             },
             new Game
             {
                 Id = 2,
                 Name = "Assassin's Creed",
                 Price = 19.99M,
                 ReleaseDate = new DateOnly(2007, 11, 13),
                 GenreId = 1 // Action
             },
             new Game
             {
                 Id = 3,
                 Name = "Horizon Zero Dawn",
                 Price = 199.99M,
                 ReleaseDate = new DateOnly(2017, 02, 28),
                 GenreId = 3 // RPG
             });
   }
}