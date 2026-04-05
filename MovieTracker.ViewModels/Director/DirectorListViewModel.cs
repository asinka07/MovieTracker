using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.ViewModels.Director
{
    public class DirectorListViewModel
    {
        public IEnumerable<DirectorIndexViewModel> Directors { get; set; } = new List<DirectorIndexViewModel>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
