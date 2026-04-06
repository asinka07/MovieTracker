using MovieTracker.ViewModels.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.ViewModels.Movies
{
    public class MovieFullDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string GenreName { get; set; }
        public string Description { get; set; }
        public DateTime Published { get; set; }
        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
        public bool IsInWatchlist { get; set; }

        public string? DirectorName { get; set; }
    }
}
