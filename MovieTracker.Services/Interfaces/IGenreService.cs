using MovieTracker.ViewModels.Genre;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreListViewModel>> GetAllAsync();
        Task<bool> CreateAsync(CreateGenreViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
