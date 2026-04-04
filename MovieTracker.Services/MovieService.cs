using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Movies;
using MovieTracker.ViewModels.Review;

namespace MovieTracker.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _dbContext;

        public MovieService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MovieIndexViewModel> GetAllAsync(int? genreId, string sortedMovies, bool isAdmin)
        {
            var query = _dbContext.Movies
                .Include(m => m.Genre)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(m => m.IsApproved);
            }

            if (genreId.HasValue)
                query = query.Where(m => m.GenreId == genreId);

            query = sortedMovies == "date_desc"
                ? query.OrderByDescending(m => m.Published)
                : query.OrderBy(m => m.Published);

            var movies = await query
                .Select(m => new MoviePartialViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    GenreName = m.Genre.Name,
                    Published = m.Published,
                    GenreId = m.GenreId,
                    SortedMovies = sortedMovies,
                    IsApproved = m.IsApproved
                })
                .ToListAsync();

            return new MovieIndexViewModel
            {
                Movies = movies,
                GenreId = genreId,
                SortedMovies = sortedMovies,
                Genres = await _dbContext.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToListAsync()
            };
        }

        public async Task<MovieFullDetailsViewModel> GetDetailsAsync(int id)
        {
            var movie = await _dbContext.Movies
                .Include(m => m.Genre)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return null;

            return new MovieFullDetailsViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreName = movie.Genre.Name,
                Description = movie.Description,
                Published = movie.Published,
                Reviews = movie.Reviews
                    .Select(r => new ReviewViewModel { Comment = r.Comment })
                    .ToList()
            };
        }

        public async Task<MovieDetailsModalViewModel> GetModalDetailsAsync(int id)
        {
            var movie = await _dbContext.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return null;

            return new MovieDetailsModalViewModel
            {
                Title = movie.Title,
                GenreName = movie.Genre.Name,
                Published = movie.Published,
                Description = movie.Description
            };
        }

        public async Task<CreateEditMovieViewModel> GetForCreateAsync()
        {
            return new CreateEditMovieViewModel
            {
                Genres = await _dbContext.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToListAsync()
            };
        }

        public async Task<CreateEditMovieViewModel> GetForEditAsync(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);
            if (movie == null) return null;

            return new CreateEditMovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                GenreId = movie.GenreId,
                Genres = await _dbContext.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToListAsync()
            };
        }

        public async Task CreateAsync(CreateEditMovieViewModel model, string userId, bool isApproved)
        {
            var movie = new Movie
            {
                Title = model.Title,
                Description = model.Description,
                GenreId = model.GenreId,
                Published = DateTime.Now,
                AddedByUserId = userId,
                IsApproved = isApproved
            };

            _dbContext.Add(movie);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ApproveMovieAsync(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie != null)
            {
                movie.IsApproved = true;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task EditAsync(CreateEditMovieViewModel model)
        {
            var movie = await _dbContext.Movies.FindAsync(model.Id);
            if (movie == null) return;

            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.GenreId = model.GenreId;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);
            if (movie != null)
            {
                _dbContext.Movies.Remove(movie);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddReviewAsync(int movieId, string comment)
        {
            var review = new Review
            {
                MovieId = movieId,
                Comment = comment
            };

            _dbContext.Reviews.Add(review);
            await _dbContext.SaveChangesAsync();
        }
    }
}