using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.ViewModels.Director
{
    public class DirectorIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Biography { get; set; }
    }
}
