using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Genre;

namespace MovieTracker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllAsync();
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
            if (!ModelState.IsValid)
                return View(model);

            bool created = await _genreService.CreateAsync(model);

            if (!created)
            {
                ModelState.AddModelError("Name", "This genre already exists.");
                return View(model);
            }

            TempData["SuccessMessage"] = $"\"{model.Name}\" added successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _genreService.DeleteAsync(id);

            if (!deleted)
            {
                TempData["ErrorMessage"] = "Genre not found.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Genre deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
