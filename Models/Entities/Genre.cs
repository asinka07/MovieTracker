using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, type genre name!")]
        [StringLength(100, ErrorMessage = "Genre cannot be more than 100 symbols")]
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
