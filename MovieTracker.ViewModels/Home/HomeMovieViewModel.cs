using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.ViewModels.Home
{
    public class HomeMovieViewModel
    {
        public int Id { get; set; }
        public DateTime Published { get; set; }
        public string Title { get; set; }
        public string GenreName { get; set; }
    }
}
