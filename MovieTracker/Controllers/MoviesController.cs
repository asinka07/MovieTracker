using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.ViewModels.Home;
using MovieTracker.ViewModels.Movies;
using MovieTracker.ViewModels.Review;

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
            var query = _dbcontext.Movies
                .Include(m => m.Genre)
                .AsQueryable();

            if (genreId.HasValue)
            {
                query = query.Where(m => m.GenreId == genreId);
            }

            query = sortedMovies == "date_desc"
                ? query.OrderByDescending(m => m.Published)
                : query.OrderBy(m => m.Published);

            var movies = await query
                .Select(m => new MoviePartialViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    GenreName = m.Genre.Name,
                    Published = m.Published,
                    GenreId = m.GenreId,
                    SortedMovies = sortedMovies
                })
                .ToListAsync();

            var viewModel = new MovieIndexViewModel
            {
                Movies = movies,
                GenreId = genreId,
                SortedMovies = sortedMovies,
                Genres = await _dbcontext.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id, int? genreId, string sortedMovies)
        {
            if (id == null) return NotFound();

            var movie = await _dbcontext.Movies
                .Include(m => m.Genre)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            var viewModel = new MovieFullDetailsViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreName = movie.Genre.Name,
                Description = movie.Description,
                Published = movie.Published,
                Reviews = movie.Reviews
                    .Select(r => new ReviewViewModel
                    {
                        Comment = r.Comment
                    })
                    .ToList()
            };

            ViewData["GenreId"] = genreId;
            ViewData["SortedMovies"] = sortedMovies;

            return View(viewModel);
        }

        public async Task<IActionResult> DetailsModal(int id)
        {
            var movie = await _dbcontext.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var model = new MovieDetailsModalViewModel
            {
                Title = movie.Title,
                GenreName = movie.Genre.Name,
                Published = movie.Published,
                Description = movie.Description
            };

            return PartialView("_MovieDetailsPartialView", model);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateEditMovieViewModel
            {
                Genres = _dbcontext.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _dbcontext.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToList();

                return View(model);
            }

            var movie = new Movie
            {
                Title = model.Title,
                Description = model.Description,
                GenreId = model.GenreId,
                Published = DateTime.Now
            };

            _dbcontext.Add(movie);
            await _dbcontext.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Movie \"{movie.Title}\" added successfully!";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _dbcontext.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            var viewModel = new CreateEditMovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                GenreId = movie.GenreId,
                Genres = _dbcontext.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEditMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _dbcontext.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToList();

                return View(model);
            }

            var movie = await _dbcontext.Movies.FindAsync(model.Id);
            if (movie == null) return NotFound();

            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.GenreId = model.GenreId;

            await _dbcontext.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Movie \"{movie.Title}\" updated successfully!";
            return RedirectToAction(nameof(Details), new { id = movie.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int movieId, string comment, int? genreId, string sortedMovies)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return RedirectToAction("Details", new { id = movieId, genreId, sortedMovies });
            }

            var review = new Review
            {
                MovieId = movieId,
                Comment = comment
            };

            _dbcontext.Reviews.Add(review);
            await _dbcontext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your review is published!";

            return RedirectToAction("Details", new { id = movieId, genreId, sortedMovies });
        }
    }

}