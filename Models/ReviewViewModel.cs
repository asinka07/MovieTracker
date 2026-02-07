using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Models
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        [Required]
        public int MovieId {  get; set; }
        public MovieViewModel Movie { get; set; }

        [Required(ErrorMessage = "Please, type a comment!")]
        [StringLength(300, ErrorMessage = "Comment length can be maximum 300 characters.")]
        public string Comment {  get; set; }
    }
}
