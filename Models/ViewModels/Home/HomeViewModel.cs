using MovieTracker.Models.Entities;

namespace MovieTracker.Models.ViewModels.Home
{
    public class HomeViewModel
    {
        public IEnumerable<Movie> LatestMovies { get; set; } = new List<Movie>();
        public List<Genre> TopGenres { get; set; } = new List<Genre>();
    }
}
