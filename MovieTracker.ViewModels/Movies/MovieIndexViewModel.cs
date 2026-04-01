using Microsoft.AspNetCore.Mvc.Rendering;
using MovieTracker.Data.Models;

namespace MovieTracker.ViewModels.Movies
{
    public class MovieIndexViewModel
    {
        public IEnumerable<MoviePartialViewModel> Movies { get; set; }

        public int? GenreId { get; set; }
        public string SortedMovies { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; }
    }
}