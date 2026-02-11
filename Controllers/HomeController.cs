using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Models;
using MovieTracker.Models.ViewModels.Home;
using System.Diagnostics;

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
                LatestMovies = await _dbcontext.Movies.Include(m => m.Genre).OrderByDescending(m => m.Published).Take(5).ToListAsync(),
                TopGenres = await _dbcontext.Genres.Include(g => g.Movies).OrderByDescending(g => g.Movies.Count).Take(5).ToListAsync()
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
