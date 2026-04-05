using Microsoft.AspNetCore.Mvc;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Director;

namespace MovieTracker.Controllers
{
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

        [HttpGet]
        public IActionResult Create()
        {
            var model = new DirectorFormViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DirectorFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _directorService.AddAsync(model);

                TempData["SuccessMessage"] = "Director added successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving the director.");
                return View(model);
            }
        }
    }
}