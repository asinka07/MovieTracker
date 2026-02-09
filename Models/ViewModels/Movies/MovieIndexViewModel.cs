using Microsoft.AspNetCore.Mvc.Rendering;
using MovieTracker.Models.Entities;

namespace MovieTracker.Models.ViewModels.Movies
{
    public class MovieIndexViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }

        public int? GenreId { get; set; }
        public IEnumerable<SelectListItem> Genres { get; set; }

        public string SortedMovies { get; set; }
    }
}
