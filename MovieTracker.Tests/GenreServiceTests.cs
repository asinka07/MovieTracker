using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Services;
using MovieTracker.ViewModels.Genre;

namespace MovieTracker.Tests
{
    public class GenreServiceTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllGenres()
        {
            var context = CreateInMemoryContext();
            context.Genres.AddRange(
                new Genre { Name = "Action" },
                new Genre { Name = "Comedy" },
                new Genre { Name = "Drama" }
            );
            await context.SaveChangesAsync();
            var service = new GenreService(context);
            var result = await service.GetAllAsync();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCorrectNames()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Name = "Action" });
            await context.SaveChangesAsync();
            var service = new GenreService(context);
            var result = await service.GetAllAsync();
            Assert.Contains(result, g => g.Name == "Action");
        }

        [Fact]
        public async Task CreateAsync_AddsGenreSuccessfully()
        {
            var context = CreateInMemoryContext();
            var service = new GenreService(context);
            var model = new CreateGenreViewModel { Name = "Horror" };
            bool result = await service.CreateAsync(model);
            Assert.True(result);
            Assert.Equal(1, context.Genres.Count());
        }

        [Fact]
        public async Task CreateAsync_ReturnsFalse_WhenGenreExists()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Name = "Horror" });
            await context.SaveChangesAsync();
            var service = new GenreService(context);
            var model = new CreateGenreViewModel { Name = "Horror" };
            bool result = await service.CreateAsync(model);
            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFalse_WhenGenreExistsCaseInsensitive()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Name = "horror" });
            await context.SaveChangesAsync();
            var service = new GenreService(context);
            var model = new CreateGenreViewModel { Name = "HORROR" };
            bool result = await service.CreateAsync(model);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_DeletesGenreSuccessfully()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Id = 1, Name = "Action" });
            await context.SaveChangesAsync();
            var service = new GenreService(context);
            bool result = await service.DeleteAsync(1);
            Assert.True(result);
            Assert.Equal(0, context.Genres.Count());
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenGenreNotFound()
        {
            var context = CreateInMemoryContext();
            var service = new GenreService(context);
            bool result = await service.DeleteAsync(999);
            Assert.False(result);
        }
    }
}