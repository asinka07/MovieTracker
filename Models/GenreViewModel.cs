using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Models
{
    public class GenreViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, type genre name!")]
        [StringLength(100, ErrorMessage = "Genre cannot be more than 100 symbols")]
        public string Name { get; set; }

        public ICollection<MovieViewModel> Movies { get; set; }
    }
}
