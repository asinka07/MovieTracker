using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.GCommon
{
    public static class EntityValidations
    {
        public const int MovieTitleMaxLength = 100;
        public const int MovieDescriptionMaxLength = 500;
        public const int MovieGenreIdMinValue = 1;

        public const int GenreNameMaxLength = 100;

        public const int ReviewCommentMaxLength = 300;
    }
}
