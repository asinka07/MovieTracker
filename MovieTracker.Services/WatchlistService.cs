using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Home;
using MovieTracker.ViewModels.Watchlist;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly ApplicationDbContext _context;

        public WatchlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(string userId, int movieId)
        {
            bool exists = await _context.Watchlists
                .AnyAsync(w => w.UserId == userId && w.MovieId == movieId);

            if (!exists)
            {
                _context.Watchlists.Add(new Watchlist
                {
                    UserId = userId,
                    MovieId = movieId,
                    AddedOn = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(string userId, int movieId)
        {
            var entry = await _context.Watchlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.MovieId == movieId);

            if (entry != null)
            {
                _context.Watchlists.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<WatchlistViewModel>> GetAllAsync(string userId)
        {
            return await _context.Watchlists
                .Where(w => w.UserId == userId)
                .Select(w => new WatchlistViewModel
                {
                    MovieId = w.MovieId,
                    Title = w.Movie.Title,
                    GenreName = w.Movie.Genre.Name,
                    AddedOn = w.AddedOn
                })
                .ToListAsync();
        }

        public async Task<bool> IsInWatchlistAsync(string userId, int movieId)
        {
            return await _context.Watchlists
                .AnyAsync(w => w.UserId == userId && w.MovieId == movieId);
        }
    }
}
