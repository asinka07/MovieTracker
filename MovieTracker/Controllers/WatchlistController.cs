using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTracker.Controllers
{
    [Authorize]
    public class WatchlistController : Controller
    {
        private readonly IWatchlistService _watchlistService;
        private readonly UserManager<IdentityUser> _userManager;

        public WatchlistController(IWatchlistService watchlistService, UserManager<IdentityUser> userManager)
        {
            _watchlistService = watchlistService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);
            var movies = await _watchlistService.GetAllAsync(userId);
            return View(movies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int movieId)
        {
            string userId = _userManager.GetUserId(User);
            await _watchlistService.AddAsync(userId, movieId);
            TempData["SuccessMessage"] = "Movie added to watchlist!";
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int movieId)
        {
            string userId = _userManager.GetUserId(User);
            await _watchlistService.RemoveAsync(userId, movieId);
            TempData["SuccessMessage"] = "Movie removed from watchlist!";
            return RedirectToAction(nameof(Index));
        }
    }
}
