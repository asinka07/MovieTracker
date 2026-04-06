using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static MovieTracker.GCommon.EntityValidations;

namespace MovieTracker.Data.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Required(ErrorMessage = "Please, type a comment!")]
        [StringLength(ReviewCommentMaxLength, ErrorMessage = "Comment length can be maximum 300 characters.")]
        public string Comment { get; set; }

        public string? UserId { get; set; }      
        public IdentityUser? User { get; set; }
    }
}