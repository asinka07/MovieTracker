using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Models
{
    public class Movie
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please, type movie title!")]
        [StringLength(200, ErrorMessage = "Title cannot be more than 200 symbols")]
        public string Title { get; set; }

        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        [Required]
        public DateTime Published { get; set; }

        [StringLength(500, ErrorMessage = "The description is too long. Max length - 500 characters")]
        public string Description {  get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
