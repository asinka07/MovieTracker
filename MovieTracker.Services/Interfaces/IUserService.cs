using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTracker.ViewModels.Admin;

namespace MovieTracker.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDetailsViewModel>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(string userId);
        Task<AdminDashboardViewModel> GetDashboardStatsAsync();
        Task<bool> PromoteAsync(string userId);    
        Task<bool> DemoteAsync(string userId);
        Task<UserDetailsViewModel?> GetUserDetailsAsync(string userId);
    }
}
