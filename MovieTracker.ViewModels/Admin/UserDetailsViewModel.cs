using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.ViewModels.Admin
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Role { get; set; }
        public int MoviesCount { get; set; }
        public int ReviewsCount { get; set; }
    }
}
