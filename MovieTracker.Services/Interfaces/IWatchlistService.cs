using MovieTracker.ViewModels.Watchlist;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task AddAsync(string userId, int movieId);
        Task RemoveAsync(string userId, int movieId);
        Task<IEnumerable<WatchlistViewModel>> GetAllAsync(string userId);
        Task<bool> IsInWatchlistAsync(string userId, int movieId);
    }
}
