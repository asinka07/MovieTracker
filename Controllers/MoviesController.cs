using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Models.Entities;

namespace MovieTracker.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        public MoviesController(ApplicationDbContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _dbcontext.Movies.Include(m => m.Genre).Include(m => m.Reviews).ToListAsync();
            return View(movies);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }
            var movie = await _dbcontext.Movies.Include(m => m.Genre).Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) { return NotFound(); }

            return View(movie);
        }

        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_dbcontext.Genres, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Published = DateTime.Now;
                _dbcontext.Add(movie);
                await _dbcontext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Movie \"{movie.Title}\" added successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_dbcontext.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _dbcontext.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            ViewData["GenreId"] = new SelectList(_dbcontext.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _dbcontext.Update(movie);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("Details", new { id = movie.Id });
            }
            ViewData["GenreId"] = new SelectList(_dbcontext.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _dbcontext.Movies.FindAsync(id);
            if (movie != null)
            {
                _dbcontext.Movies.Remove(movie);
                await _dbcontext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}