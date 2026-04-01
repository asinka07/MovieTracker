using MovieTracker.Data.Models;

namespace MovieTracker.ViewModels.Home
{
    public class HomeViewModel
    {
        public IEnumerable<HomeMovieViewModel> LatestMovies { get; set; }
        public List<HomeGenreViewModel> TopGenres { get; set; }
    }
}