using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MovieTracker.GCommon.EntityValidations;

namespace MovieTracker.ViewModels.Director
{
    public class DirectorFormViewModel
    {

        [Required(ErrorMessage = "Director name is mandatory.")]
        [StringLength(DirectorNameMaxLength, MinimumLength = DirectorNameMinLength,
            ErrorMessage = "The name must be between {2} and {1} characters long.")]
        public string Name { get; set; } = null!;

        [StringLength(DirectorBiographyMaxLength, MinimumLength = DirectorBiographyMinLength,
            ErrorMessage = "Biography must be between {2} and {1} characters.")]
        public string? Biography { get; set; }
    }
}
