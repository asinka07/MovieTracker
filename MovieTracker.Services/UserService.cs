using MovieTracker.Data;
using MovieTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTracker.ViewModels.Admin; 

namespace MovieTracker.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<UserDetailsViewModel>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UserDetailsViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var moviesCount = await _context.Movies.CountAsync(m => m.AddedByUserId == user.Id);
                var reviewsCount = await _context.Reviews.CountAsync(r => r.UserId == user.Id);

                result.Add(new UserDetailsViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    EmailConfirmed = user.EmailConfirmed,
                    Role = roles.FirstOrDefault() ?? "User",
                    MoviesCount = moviesCount,
                    ReviewsCount = reviewsCount
                });
            }

            return result;
        }

        public async Task<AdminDashboardViewModel> GetDashboardStatsAsync()
        {
            return new AdminDashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalMovies = await _context.Movies.CountAsync(),
                PendingMovies = await _context.Movies.CountAsync(m => !m.IsApproved),
                TotalReviews = await _context.Reviews.CountAsync(),
                TotalDirectors = await _context.Directors.CountAsync(),
                TotalGenres = await _context.Genres.CountAsync()
            };
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> PromoteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            await _userManager.AddToRoleAsync(user, "Administrator");
            return true;
        }

        public async Task<bool> DemoteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var admins = await _userManager.GetUsersInRoleAsync("Administrator");
            if (admins.Count == 1) return false;

            await _userManager.RemoveFromRoleAsync(user, "Administrator");
            return true;
        }

        public async Task<UserDetailsViewModel?> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var moviesCount = await _context.Movies.CountAsync(m => m.AddedByUserId == userId);
            var reviewsCount = await _context.Reviews.CountAsync(r => r.UserId == userId);

            return new UserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
                Role = roles.FirstOrDefault() ?? "User",
                MoviesCount = moviesCount,
                ReviewsCount = reviewsCount
            };
        }
    }
}
