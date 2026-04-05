using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Director;

namespace MovieTracker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class DirectorsController : Controller
    {
        private readonly IDirectorService _directorService;

        public DirectorsController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        public async Task<IActionResult> Index()
        {
            var directors = await _directorService.GetAllAsync();
            return View(directors);
        }

        public IActionResult Create()
        {
            return View(new DirectorFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DirectorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _directorService.AddAsync(model);
            TempData["SuccessMessage"] = "Director added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _directorService.GetForEditAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DirectorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _directorService.EditAsync(model);
            TempData["SuccessMessage"] = "Director updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _directorService.DeleteAsync(id);

            if (!deleted)
            {
                TempData["ErrorMessage"] = "Director not found.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Director deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}