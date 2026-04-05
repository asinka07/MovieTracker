using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Movies;

namespace MovieTracker.Controllers
{
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
            bool isAdmin = User.IsInRole("Administrator");

            var viewModel = await _movieService.GetAllAsync(genreId, sortedMovies, isAdmin);
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id, int? genreId, string sortedMovies)
        {
            if (id == null) return NotFound();

            var viewModel = await _movieService.GetDetailsAsync(id.Value);
            if (viewModel == null) return NotFound();

            ViewData["GenreId"] = genreId;
            ViewData["SortedMovies"] = sortedMovies;

            return View(viewModel);
        }

        public async Task<IActionResult> DetailsModal(int id)
        {
            var model = await _movieService.GetModalDetailsAsync(id);
            if (model == null) return NotFound();

            return PartialView("_MovieDetailsPartialView", model);
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
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["ErrorMessage"] = string.Join(" | ", errors);

                var viewModel = await _movieService.GetForCreateAsync();
                model.Genres = viewModel.Genres;
                return View(model);
            }

            string userId = _userManager.GetUserId(User);
            bool isApproved = User.IsInRole("Administrator");

            await _movieService.CreateAsync(model, userId, isApproved);

            if (isApproved)
                TempData["SuccessMessage"] = $"Movie \"{model.Title}\" added successfully!";
            else
                TempData["SuccessMessage"] = $"Movie \"{model.Title}\" has been submitted for admin approval!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int movieId)
        {
            await _movieService.ApproveMovieAsync(movieId);
            TempData["SuccessMessage"] = "The movie has been approved!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
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
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddReview(int movieId, string comment, int? genreId, string sortedMovies)
        {
            if (!string.IsNullOrWhiteSpace(comment))
                await _movieService.AddReviewAsync(movieId, comment);

            TempData["SuccessMessage"] = "Your review is published!";
            return RedirectToAction("Details", new { id = movieId, genreId, sortedMovies });
        }
    }
}