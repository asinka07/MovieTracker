using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please, enter a movie title!")]
        [StringLength(100, ErrorMessage = "Title cannot be more than 100 symbols")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select a genre!")]
        [Range(1, int.MaxValue, ErrorMessage ="Please, select a genre")]
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        public DateTime Published { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The description field is required!")]
        [StringLength(500, ErrorMessage = "The description is too long. Max length - 500 characters")]
        public string Description {  get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
