using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.ViewModels.Movies
{
    public class MoviePartialViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string GenreName { get; set; }
        public DateTime Published { get; set; }
        public int? GenreId { get; set; }
        public string SortedMovies { get; set; }
        public bool IsApproved { get; set; }
        public string AddedByUserName { get; set; }
    }

}
