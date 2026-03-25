using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Models.ViewModels.Movies;

namespace MovieTracker.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        public MoviesController(ApplicationDbContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public async Task<IActionResult> Index(int? genreId, string sortedMovies)
        {
            var movies = _dbcontext.Movies.Include(m => m.Genre).AsQueryable();
            if (genreId.HasValue)
            {
                movies = movies.Where(m => m.GenreId == genreId);
            }

            movies = sortedMovies == "date_desc" ? movies.OrderByDescending(m => m.Published) : movies.OrderBy(m => m.Published);

            var viewModel = new MovieIndexViewModel
            {
                Movies = await movies.ToListAsync(),
                GenreId = genreId,
                SortedMovies = sortedMovies,
                Genres = await _dbcontext.Genres.Select(g => new SelectListItem{Value = g.Id.ToString(), Text = g.Name}).ToListAsync()
            };

            return View(viewModel);

        }

        public async Task<IActionResult> Details(int? id, int? genreId, string sortedMovies)
        {
            if (id == null) { return NotFound(); }
            var movie = await _dbcontext.Movies.Include(m => m.Genre).Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) { return NotFound(); }

            ViewData["GenreId"] = genreId;
            ViewData["SortedMovies"] = sortedMovies;

            return View(movie);
        }

        public async Task<IActionResult> DetailsModal(int id)
        {
            var movie = await _dbcontext.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound();
            return PartialView("~/Views/Shared/_MovieDetailsPartialView.cshtml", movie);
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
                var originalMovie = await _dbcontext.Movies.FindAsync(id);
                if (originalMovie == null) return NotFound();
                originalMovie.Title = movie.Title;
                originalMovie.Description = movie.Description;
                originalMovie.GenreId = movie.GenreId;
                await _dbcontext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Movie \"{originalMovie.Title}\" updated successfully!";
                return RedirectToAction("Details", new { id = originalMovie.Id });
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


        public async Task<IActionResult> AddReview(int MovieId, string Comment, int? genreId, string sortedMovies)
        {
            if (!string.IsNullOrWhiteSpace(Comment))
            {
                _dbcontext.Reviews.Add(new Review { MovieId = MovieId, Comment = Comment });
            await _dbcontext.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Your review is published!";
            return RedirectToAction("Details", new { id = MovieId, genreId, sortedMovies });
        }
    }

}