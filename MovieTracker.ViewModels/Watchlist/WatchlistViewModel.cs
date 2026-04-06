using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.ViewModels.Watchlist
{
    public class WatchlistViewModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string GenreName { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
