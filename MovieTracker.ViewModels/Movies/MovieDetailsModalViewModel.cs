using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.ViewModels.Movies
{
    public class MovieDetailsModalViewModel
    {
        public string Title { get; set; }
        public string GenreName { get; set; }
        public DateTime Published { get; set; }
        public string Description { get; set; }
    }
}
