using System.ComponentModel.DataAnnotations;
using static MovieTracker.GCommon.EntityValidations;


namespace MovieTracker.Data.Models
{
    public class Movie
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please, enter a movie title!")]
        [StringLength(MovieTitleMaxLength, ErrorMessage = "Title cannot be more than 100 symbols")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select a genre!")]
        [Range(MovieGenreIdMinValue, int.MaxValue, ErrorMessage = "Please, select a genre")]
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        public DateTime Published { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The description field is required!")]
        [StringLength(MovieDescriptionMaxLength, ErrorMessage = "The description is too long. Max length - 500 characters")]
        public string Description { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}