using System.ComponentModel.DataAnnotations;
using static MovieTracker.GCommon.EntityValidations;

namespace MovieTracker.Data.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, type genre name!")]
        [StringLength(GenreNameMaxLength, ErrorMessage = "Genre cannot be more than 100 symbols")]
        public string Name { get; set; }

        public ICollection<Movie>? Movies { get; set; }
    }
}