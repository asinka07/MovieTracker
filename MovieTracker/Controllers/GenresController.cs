using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Genre;
using Microsoft.AspNetCore.Authorization;

namespace MovieTracker.Controllers
{
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

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool created = await _genreService.CreateAsync(model);

                if (!created)
                {
                    ModelState.AddModelError("Name", "This genre already exists.");
                    return View(model);
                }

                TempData["SuccessMessage"] = $"\"{model.Name}\" added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}