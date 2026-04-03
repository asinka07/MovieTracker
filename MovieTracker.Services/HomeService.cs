using MovieTracker.Data;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.Services
{
    public class HomeService:IHomeService
    {
        private readonly ApplicationDbContext _dbcontext;

        public HomeService(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<HomeViewModel> GetHomeDataAsync()
        {
            var model = new HomeViewModel
            {
                LatestMovies = await _dbcontext.Movies
                    .OrderByDescending(m => m.Published)
                    .Take(5)
                    .Select(m => new HomeMovieViewModel
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Published = m.Published,
                        GenreName = m.Genre.Name
                    })
                    .ToListAsync(),

                TopGenres = await _dbcontext.Genres
                    .OrderByDescending(g => g.Movies.Count)
                    .Take(5)
                    .Select(g => new HomeGenreViewModel
                    {
                        Id = g.Id,
                        Name = g.Name,
                        MoviesCount = g.Movies.Count
                    })
                    .ToListAsync()
            };

            return model;
        }
    }
}
