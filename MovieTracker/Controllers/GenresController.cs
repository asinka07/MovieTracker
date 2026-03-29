using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MovieTracker.Data.Models;
using MovieTracker.Data;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre)
        {
            bool genreExists = await _dbcontext.Genres.AnyAsync(g => g.Name.ToLower() == genre.Name.ToLower());

            if (genreExists)
            {
                ModelState.AddModelError("Name", "This genre already exists.");
            }
            if (ModelState.IsValid)
            {
                _dbcontext.Add(genre);
                await _dbcontext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"\"{genre.Name}\" added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }
    }
}