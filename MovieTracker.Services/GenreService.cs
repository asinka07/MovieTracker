using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Genre;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _dbContext;

        public GenreService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GenreListViewModel>> GetAllAsync()
        {
            return await _dbContext.Genres
                .Select(g => new GenreListViewModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(CreateGenreViewModel model)
        {
            bool genreExists = await _dbContext.Genres
                .AnyAsync(g => g.Name.ToLower() == model.Name.ToLower());

            if (genreExists) return false;

            var genre = new Genre { Name = model.Name };
            _dbContext.Add(genre);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
