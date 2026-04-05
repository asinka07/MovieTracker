using MovieTracker.Data.Models;
using MovieTracker.ViewModels.Director;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.Services.Interfaces
{
    public interface IDirectorService
    {
        Task<IEnumerable<DirectorIndexViewModel>> GetAllAsync();
        Task<Director?> GetByIdAsync(int id);
        Task AddAsync(DirectorFormViewModel director);
        Task<bool> ExistsAsync(int id);
    }
}
