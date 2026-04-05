using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MovieTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            var adminEmail = "admin@test.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                await userManager.CreateAsync(adminUser, "MovieTracker2@26");
                await userManager.AddToRoleAsync(adminUser, "Administrator");
            }

            var genres = new List<Genre>
            {
                new Genre { Name = "Action" },
                new Genre { Name = "Animation" },
                new Genre { Name = "Documentary" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Comedy" },
                new Genre { Name = "Rom-Com" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Sci-Fi" }
            };

            foreach (var genre in genres)
            {
                if (!context.Genres.Any(g => g.Name == genre.Name))
                {
                    context.Genres.Add(genre);
                }
            }
            await context.SaveChangesAsync();

            var directors = new List<Director>
            {
                new Director
                {
                    Name = "Christopher Nolan",
                    Biography = "Christopher Edward Nolan is a British-American filmmaker. Known for his Hollywood blockbusters with complex storytelling, Nolan is considered a leading filmmaker of his generation. His films have grossed more than $5 billion worldwide. Among his most notable works are Inception, Interstellar, The Dark Knight trilogy, and Oppenheimer."
                },
                new Director
                {
                    Name = "Steven Spielberg",
                    Biography = "Steven Allan Spielberg is an American filmmaker. A major figure of the New Hollywood era and pioneer of the modern blockbuster, Spielberg is widely regarded as one of the greatest and most influential filmmakers in the history of cinema. Notable works include Schindler's List, Jurassic Park, and Saving Private Ryan."
                },
                new Director
                {
                    Name = "Martin Scorsese",
                    Biography = "Martin Charles Scorsese is an American filmmaker who emerged as one of the major figures of the New Hollywood era. His films include Taxi Driver, Raging Bull, Goodfellas, The Departed, and The Wolf of Wall Street."
                },
                new Director
                {
                    Name = "Quentin Tarantino",
                    Biography = "Quentin Jerome Tarantino is an American filmmaker and actor. His films are characterized by stylized violence, extended dialogue, and references to popular culture. Notable works include Pulp Fiction, Kill Bill, Django Unchained, and Inglourious Basterds."
                },
                new Director
                {
                    Name = "Denis Villeneuve",
                    Biography = "Denis Villeneuve is a Canadian filmmaker known for his thought-provoking science fiction films. His works include Arrival, Blade Runner 2049, Dune, and Dune: Part Two."
                },
                new Director
                {
                    Name = "David Fincher",
                    Biography = "David Andrew Leo Fincher is an American filmmaker known for his dark and stylish thrillers. His notable works include Se7en, Fight Club, The Social Network, and Gone Girl."
                },
                new Director
                {
                    Name = "James Cameron",
                    Biography = "James Francis Cameron is a Canadian filmmaker known for pushing the boundaries of visual effects. His films include The Terminator, Titanic, and Avatar, two of the highest-grossing films of all time."
                },
                new Director
                {
                    Name = "Ridley Scott",
                    Biography = "Sir Ridley Scott is a British filmmaker and producer. He is known for his atmospheric and visually striking films. His notable works include Alien, Blade Runner, Gladiator, The Martian, and Napoleon."
                },
                new Director
                {
                    Name = "Stanley Kubrick",
                    Biography = "Stanley Kubrick was an American filmmaker regarded as one of the greatest directors in cinematic history. His films include 2001: A Space Odyssey, A Clockwork Orange, The Shining, and Full Metal Jacket."
                },
                new Director
                {
                    Name = "Francis Ford Coppola",
                    Biography = "Francis Ford Coppola is an American filmmaker and one of the major figures of the New Hollywood era. He is best known for The Godfather trilogy and Apocalypse Now, considered among the greatest films ever made."
                },
                new Director
                {
                    Name = "Alfonso Cuarón",
                    Biography = "Alfonso Cuarón is a Mexican filmmaker known for his visually innovative style. His notable works include Y Tu Mamá También, Harry Potter and the Prisoner of Azkaban, Children of Men, Gravity, and Roma."
                },
                new Director
                {
                    Name = "Guillermo del Toro",
                    Biography = "Guillermo del Toro is a Mexican filmmaker known for his dark fantasy and horror films. His works include Pan's Labyrinth, Hellboy, The Shape of Water, which won the Academy Award for Best Picture, and Pinocchio."
                },
                new Director
                {
                    Name = "Wes Anderson",
                    Biography = "Wesley Wales Anderson is an American filmmaker known for his distinctive visual and narrative style. His films include The Royal Tenenbaums, The Grand Budapest Hotel, Isle of Dogs, and Asteroid City."
                },
                new Director
                {
                    Name = "Tim Burton",
                    Biography = "Timothy Walter Burton is an American filmmaker known for his gothic fantasy and horror films. His notable works include Beetlejuice, Edward Scissorhands, Batman, Sleepy Hollow, and Big Eyes."
                },
                new Director
                {
                    Name = "Spike Lee",
                    Biography = "Shelton Jackson Lee is an American filmmaker and one of the most influential directors in Hollywood. His works include Do the Right Thing, Malcolm X, 25th Hour, and BlacKkKlansman, which won the Grand Prix at Cannes."
                },
                new Director
                {
                    Name = "Bong Joon-ho",
                    Biography = "Bong Joon-ho is a South Korean filmmaker and screenwriter. He became the first South Korean director to win the Palme d'Or at Cannes. His films include Memories of Murder, The Host, Snowpiercer, and Parasite."
                },
                new Director
                {
                    Name = "Akira Kurosawa",
                    Biography = "Akira Kurosawa was a Japanese filmmaker widely regarded as one of the most influential directors in the history of cinema. His works include Rashomon, Seven Samurai, Yojimbo, and Ran."
                },
                new Director
                {
                    Name = "Peter Jackson",
                    Biography = "Sir Peter Robert Jackson is a New Zealand filmmaker best known for his epic fantasy adaptations. His works include The Lord of the Rings trilogy, The Hobbit trilogy, and King Kong."
                }
            };

            foreach (var director in directors)
            {
                if (!context.Directors.Any(d => d.Name == director.Name))
                {
                    context.Directors.Add(director);
                }
            }
            await context.SaveChangesAsync();

        }
    }
}
