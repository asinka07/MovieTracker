using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Movies;

namespace MovieTracker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly UserManager<IdentityUser> _userManager;

        public MoviesController(IMovieService movieService, UserManager<IdentityUser> userManager)
        {
            _movieService = movieService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? genreId, string sortedMovies)
        {
            var viewModel = await _movieService.GetAllAsync(genreId, sortedMovies, isAdmin: true);
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = await _movieService.GetForCreateAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _movieService.GetForCreateAsync();
                model.Genres = viewModel.Genres;
                return View(model);
            }

            string userId = _userManager.GetUserId(User);
            await _movieService.CreateAsync(model, userId, isApproved: true);
            TempData["SuccessMessage"] = $"Movie \"{model.Title}\" added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _movieService.GetForEditAsync(id);
            if (viewModel == null) return NotFound();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEditMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _movieService.GetForEditAsync(model.Id);
                model.Genres = viewModel.Genres;
                return View(model);
            }

            await _movieService.EditAsync(model);
            TempData["SuccessMessage"] = $"Movie \"{model.Title}\" updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int movieId, string returnAction = "Index")
        {
            await _movieService.ApproveMovieAsync(movieId);
            TempData["SuccessMessage"] = "The movie has been approved!";
            return RedirectToAction(returnAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnAction = "Index")
        {
            await _movieService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Movie deleted successfully!";
            return RedirectToAction(returnAction);
        }

        public async Task<IActionResult> Pending()
        {
            var movies = await _movieService.GetPendingAsync();
            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetDetailsAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }
    }
}