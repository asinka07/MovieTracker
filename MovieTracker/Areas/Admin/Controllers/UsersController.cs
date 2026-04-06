using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Services.Interfaces;


namespace MovieTracker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Details(string userId)
        {
            var user = await _userService.GetUserDetailsAsync(userId);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string userId)
        {
            bool deleted = await _userService.DeleteUserAsync(userId);

            if (!deleted)
            {
                TempData["ErrorMessage"] = "User not found or cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Promote(string userId)
        {
            bool promoted = await _userService.PromoteAsync(userId);

            if (!promoted)
                TempData["ErrorMessage"] = "Could not promote user.";
            else
                TempData["SuccessMessage"] = "User promoted to Administrator!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Demote(string userId)
        {
            bool demoted = await _userService.DemoteAsync(userId);

            if (!demoted)
                TempData["ErrorMessage"] = "Cannot demote the last Administrator!";
            else
                TempData["SuccessMessage"] = "User demoted successfully!";

            return RedirectToAction(nameof(Index));
        }
    }
}
