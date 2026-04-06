using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Services;
using MovieTracker.ViewModels.Movies;

namespace MovieTracker.Tests
{
    public class MovieServiceTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsMovieSuccessfully()
        {
            var context = CreateInMemoryContext();
            var service = new MovieService(context);
            var model = new CreateEditMovieViewModel
            {
                Title = "Inception",
                Description = "A mind-bending thriller.",
                GenreId = 1
            };

            await service.CreateAsync(model, "user1", true);

            Assert.Equal(1, context.Movies.Count());
            Assert.Equal("Inception", context.Movies.First().Title);
        }

        [Fact]
        public async Task CreateAsync_SetsIsApprovedCorrectly()
        {
            var context = CreateInMemoryContext();
            var service = new MovieService(context);
            var model = new CreateEditMovieViewModel
            {
                Title = "Test",
                Description = "Test description.",
                GenreId = 1
            };

            await service.CreateAsync(model, "user1", false);

            Assert.False(context.Movies.First().IsApproved);
        }

        [Fact]
        public async Task DeleteAsync_DeletesMovieSuccessfully()
        {
            var context = CreateInMemoryContext();
            context.Movies.Add(new Movie
            {
                Id = 1,
                Title = "Test",
                Description = "Test.",
                GenreId = 1
            });
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            await service.DeleteAsync(1);

            Assert.Equal(0, context.Movies.Count());
        }

        [Fact]
        public async Task ApproveMovieAsync_ApprovesMovieSuccessfully()
        {
            var context = CreateInMemoryContext();
            context.Movies.Add(new Movie
            {
                Id = 1,
                Title = "Test",
                Description = "Test.",
                GenreId = 1,
                IsApproved = false
            });
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            await service.ApproveMovieAsync(1);

            Assert.True(context.Movies.First().IsApproved);
        }

        [Fact]
        public async Task EditAsync_UpdatesMovieSuccessfully()
        {
            var context = CreateInMemoryContext();
            context.Movies.Add(new Movie
            {
                Id = 1,
                Title = "Old Title",
                Description = "Old description.",
                GenreId = 1
            });
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            var model = new CreateEditMovieViewModel
            {
                Id = 1,
                Title = "New Title",
                Description = "New description.",
                GenreId = 2
            };

            await service.EditAsync(model);

            var updated = await context.Movies.FindAsync(1);
            Assert.Equal("New Title", updated.Title);
            Assert.Equal("New description.", updated.Description);
        }

        [Fact]
        public async Task GetPendingAsync_ReturnsOnlyPendingMovies()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Id = 1, Name = "Action" });
            context.Movies.AddRange(
                new Movie { Id = 1, Title = "Approved", Description = "Test.", GenreId = 1, IsApproved = true },
                new Movie { Id = 2, Title = "Pending", Description = "Test.", GenreId = 1, IsApproved = false }
            );
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            var result = await service.GetPendingAsync();

            Assert.Single(result);
            Assert.Equal("Pending", result.First().Title);
        }

        [Fact]
        public async Task AddReviewAsync_AddsReviewSuccessfully()
        {
            var context = CreateInMemoryContext();
            var service = new MovieService(context);

            await service.AddReviewAsync(1, "Great movie!", "user1");

            Assert.Equal(1, context.Reviews.Count());
            Assert.Equal("Great movie!", context.Reviews.First().Comment);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOnlyApprovedMovies_WhenNotAdmin()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Id = 1, Name = "Action" });
            context.Movies.AddRange(
                new Movie { Id = 1, Title = "Approved", Description = "Test.", GenreId = 1, IsApproved = true },
                new Movie { Id = 2, Title = "Pending", Description = "Test.", GenreId = 1, IsApproved = false }
            );
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            var result = await service.GetAllAsync(null, null, isAdmin: false);

            Assert.Single(result.Movies);
            Assert.Equal("Approved", result.Movies.First().Title);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllMovies_WhenAdmin()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Id = 1, Name = "Action" });
            context.Movies.AddRange(
                new Movie { Id = 1, Title = "Approved", Description = "Test.", GenreId = 1, IsApproved = true },
                new Movie { Id = 2, Title = "Pending", Description = "Test.", GenreId = 1, IsApproved = false }
            );
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            var result = await service.GetAllAsync(null, null, isAdmin: true);

            Assert.Equal(2, result.Movies.Count());
        }

        [Fact]
        public async Task GetDetailsAsync_ReturnsCorrectMovie()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Id = 1, Name = "Action" });
            context.Movies.Add(new Movie
            {
                Id = 1,
                Title = "Inception",
                Description = "A thriller.",
                GenreId = 1,
                IsApproved = true
            });
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            var result = await service.GetDetailsAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
        }

        [Fact]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotFound()
        {
            var context = CreateInMemoryContext();
            var service = new MovieService(context);

            var result = await service.GetDetailsAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetForEditAsync_ReturnsCorrectMovie()
        {
            var context = CreateInMemoryContext();
            context.Movies.Add(new Movie
            {
                Id = 1,
                Title = "Inception",
                Description = "A thriller.",
                GenreId = 1
            });
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            var result = await service.GetForEditAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
        }

        [Fact]
        public async Task GetForEditAsync_ReturnsNull_WhenNotFound()
        {
            var context = CreateInMemoryContext();
            var service = new MovieService(context);

            var result = await service.GetForEditAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetModalDetailsAsync_ReturnsCorrectMovie()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Id = 1, Name = "Action" });
            context.Movies.Add(new Movie { Id = 1, Title = "Inception", Description = "A thriller.", GenreId = 1 });
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            var result = await service.GetModalDetailsAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
        }

        [Fact]
        public async Task GetModalDetailsAsync_ReturnsNull_WhenNotFound()
        {
            var context = CreateInMemoryContext();
            var service = new MovieService(context);

            var result = await service.GetModalDetailsAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetForCreateAsync_ReturnsGenresAndDirectors()
        {
            var context = CreateInMemoryContext();
            context.Genres.Add(new Genre { Id = 1, Name = "Action" });
            context.Directors.Add(new Director { Id = 1, Name = "Nolan", Biography = "Test." });
            await context.SaveChangesAsync();

            var service = new MovieService(context);
            var result = await service.GetForCreateAsync();

            Assert.NotNull(result);
            Assert.Single(result.Genres);
            Assert.Single(result.Directors);
        }
    }
}