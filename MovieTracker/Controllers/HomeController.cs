using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data.Models;
using MovieTracker.Data;
using MovieTracker.ViewModels.Movies;
using System.Diagnostics;
using MovieTracker.ViewModels;
using MovieTracker.ViewModels.Home;

namespace MovieTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;

        public HomeController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                LatestMovies = await _dbcontext.Movies.OrderByDescending(m => m.Published).Take(5).Select(m => 
                new HomeMovieViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Published = m.Published,
                    GenreName = m.Genre.Name
                }).ToListAsync(),

                TopGenres = await _dbcontext.Genres.OrderByDescending(g => g.Movies.Count).Take(5).Select(g => new HomeGenreViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    MoviesCount = g.Movies.Count
                }).ToListAsync()
            };

            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
