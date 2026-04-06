using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Services;
using MovieTracker.ViewModels.Director;

namespace MovieTracker.Tests
{
    public class DirectorServiceTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllDirectors()
        {
            var context = CreateInMemoryContext();
            context.Directors.AddRange(
                new Director { Name = "Christopher Nolan", Biography = "British-American filmmaker." },
                new Director { Name = "Steven Spielberg", Biography = "American filmmaker." }
            );
            await context.SaveChangesAsync();

            var service = new DirectorService(context);
            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsDirectorSuccessfully()
        {
            var context = CreateInMemoryContext();
            var service = new DirectorService(context);
            var model = new DirectorFormViewModel
            {
                Name = "Martin Scorsese",
                Biography = "American filmmaker."
            };

            await service.AddAsync(model);

            Assert.Equal(1, context.Directors.Count());
        }

        [Fact]
        public async Task ExistsAsync_ReturnsTrue_WhenDirectorExists()
        {
            var context = CreateInMemoryContext();
            context.Directors.Add(new Director { Id = 1, Name = "Test", Biography = "Test bio." });
            await context.SaveChangesAsync();

            var service = new DirectorService(context);
            var result = await service.ExistsAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_DeletesDirectorSuccessfully()
        {
            var context = CreateInMemoryContext();
            context.Directors.Add(new Director { Id = 1, Name = "Test", Biography = "Test bio." });
            await context.SaveChangesAsync();

            var service = new DirectorService(context);
            var result = await service.DeleteAsync(1);

            Assert.True(result);
            Assert.Equal(0, context.Directors.Count());
        }

        [Fact]
        public async Task EditAsync_UpdatesDirectorSuccessfully()
        {
            var context = CreateInMemoryContext();
            context.Directors.Add(new Director { Id = 1, Name = "Old Name", Biography = "Old bio." });
            await context.SaveChangesAsync();

            var service = new DirectorService(context);
            var model = new DirectorFormViewModel { Id = 1, Name = "New Name", Biography = "New bio." };

            await service.EditAsync(model);

            var updated = await context.Directors.FindAsync(1);
            Assert.Equal("New Name", updated.Name);
        }
    }
}