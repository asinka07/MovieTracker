using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalMovies { get; set; }
        public int PendingMovies { get; set; }
        public int TotalReviews { get; set; }
    }
}
