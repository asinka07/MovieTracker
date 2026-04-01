using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.ViewModels.Genre;

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
            var genres = await _dbcontext.Genres.Select(g => new GenreListViewModel{Id = g.Id, Name = g.Name}).ToListAsync();

            return View(genres);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreViewModel model)
        {
            bool genreExists = await _dbcontext.Genres
                .AnyAsync(g => g.Name.ToLower() == model.Name.ToLower());

            if (genreExists)
            {
                ModelState.AddModelError("Name", "This genre already exists.");
            }

            if (ModelState.IsValid)
            {
                var genre = new Genre
                {
                    Name = model.Name
                };

                _dbcontext.Add(genre);
                await _dbcontext.SaveChangesAsync();

                TempData["SuccessMessage"] = $"\"{genre.Name}\" added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}