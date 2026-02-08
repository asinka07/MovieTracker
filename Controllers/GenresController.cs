using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Models.Entities;

namespace MovieTracker.Controllers
{
    public class GenresController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        public GenresController(ApplicationDbContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _dbcontext.Genres.ToListAsync();
            return View(genres);
        }
    }
}