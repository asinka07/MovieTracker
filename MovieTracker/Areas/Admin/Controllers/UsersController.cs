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
    }
}
