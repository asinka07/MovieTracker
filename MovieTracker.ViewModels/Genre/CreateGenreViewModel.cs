using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static MovieTracker.GCommon.EntityValidations;

namespace MovieTracker.ViewModels.Genre
{
    public class CreateGenreViewModel
    {
        [Required(ErrorMessage = "Please, type genre name!")]
        [StringLength(GenreNameMaxLength, ErrorMessage = "Genre cannot be more than 100 symbols")]
        public string Name { get; set; }
    }
}
