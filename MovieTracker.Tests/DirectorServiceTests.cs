using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Services;

namespace MovieTracker.Tests
{
    public class WatchlistServiceTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddAsync_AddsMovieToWatchlist()
        {
            var context = CreateInMemoryContext();
            var service = new WatchlistService(context);

            await service.AddAsync("user1", 1);

            Assert.Equal(1, context.Watchlists.Count());
        }

        [Fact]
        public async Task AddAsync_DoesNotAddDuplicate()
        {
            var context = CreateInMemoryContext();
            context.Watchlists.Add(new Watchlist { UserId = "user1", MovieId = 1 });
            await context.SaveChangesAsync();

            var service = new WatchlistService(context);
            await service.AddAsync("user1", 1);

            Assert.Equal(1, context.Watchlists.Count());
        }

        [Fact]
        public async Task RemoveAsync_RemovesMovieFromWatchlist()
        {
            var context = CreateInMemoryContext();
            context.Watchlists.Add(new Watchlist { UserId = "user1", MovieId = 1 });
            await context.SaveChangesAsync();

            var service = new WatchlistService(context);
            await service.RemoveAsync("user1", 1);

            Assert.Equal(0, context.Watchlists.Count());
        }

        [Fact]
        public async Task IsInWatchlistAsync_ReturnsTrue_WhenMovieExists()
        {
            var context = CreateInMemoryContext();
            context.Watchlists.Add(new Watchlist { UserId = "user1", MovieId = 1 });
            await context.SaveChangesAsync();

            var service = new WatchlistService(context);
            var result = await service.IsInWatchlistAsync("user1", 1);

            Assert.True(result);
        }

        [Fact]
        public async Task IsInWatchlistAsync_ReturnsFalse_WhenMovieNotExists()
        {
            var context = CreateInMemoryContext();
            var service = new WatchlistService(context);

            var result = await service.IsInWatchlistAsync("user1", 999);

            Assert.False(result);
        }

        [Fact]
        public async Task GetForEditAsync_ReturnsCorrectDirector()
        {
            var context = CreateInMemoryContext();
            context.Directors.Add(new Director { Id = 1, Name = "Christopher Nolan", Biography = "British-American filmmaker." });
            await context.SaveChangesAsync();

            var service = new DirectorService(context);
            var result = await service.GetForEditAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Christopher Nolan", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectDirector()
        {
            var context = CreateInMemoryContext();
            context.Directors.Add(new Director { Id = 1, Name = "Christopher Nolan", Biography = "British-American filmmaker." });
            await context.SaveChangesAsync();

            var service = new DirectorService(context);
            var result = await service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Christopher Nolan", result.Name);
        }
    }
}