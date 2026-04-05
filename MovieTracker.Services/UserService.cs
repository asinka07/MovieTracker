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

        public async Task<IEnumerable<IdentityUser>> GetAllUsersAsync() => await _userManager.Users.ToListAsync();

        public async Task<AdminDashboardViewModel> GetDashboardStatsAsync()
        {
            return new AdminDashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalMovies = await _context.Movies.CountAsync(),
                PendingMovies = await _context.Movies.CountAsync(m => !m.IsApproved),
                TotalReviews = await _context.Reviews.CountAsync()
            };
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
