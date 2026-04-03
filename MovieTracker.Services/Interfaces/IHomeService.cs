using MovieTracker.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.Services.Interfaces
{
    public interface IHomeService
    {
        Task<HomeViewModel> GetHomeDataAsync();
    }
}
