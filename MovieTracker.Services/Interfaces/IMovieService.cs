using MovieTracker.ViewModels.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.Services.Interfaces
{
    public interface IMovieService
    {
        Task<MovieIndexViewModel> GetAllAsync(int? genreId, string sortedMovies);
        Task<MovieFullDetailsViewModel> GetDetailsAsync(int id);
        Task<MovieDetailsModalViewModel> GetModalDetailsAsync(int id);
        Task<CreateEditMovieViewModel> GetForCreateAsync();
        Task<CreateEditMovieViewModel> GetForEditAsync(int id);
        Task CreateAsync(CreateEditMovieViewModel model);
        Task EditAsync(CreateEditMovieViewModel model);
        Task DeleteAsync(int id);
        Task AddReviewAsync(int movieId, string comment);
    }
}
