using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static MovieTracker.GCommon.EntityValidations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieTracker.ViewModels.Movies
{
    public class CreateEditMovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, enter a movie title!")]
        [StringLength(MovieTitleMaxLength, ErrorMessage = "Title cannot be more than 100 symbols")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a genre!")]
        [Range(MovieGenreIdMinValue, int.MaxValue, ErrorMessage = "Please, select a genre")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "The description field is required!")]
        [StringLength(MovieDescriptionMaxLength, ErrorMessage = "The description is too long. Max length - 500 characters")]
        public string Description { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
    }
}
