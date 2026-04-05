using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Data.Models;
using MovieTracker.Services.Interfaces;
using MovieTracker.ViewModels.Director;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly ApplicationDbContext _context;

        public DirectorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DirectorIndexViewModel>> GetAllAsync()
        {
            return await _context.Directors
                        .Select(d => new DirectorIndexViewModel
                        {
                            Id = d.Id,
                            Name = d.Name,
                            Biography = d.Biography
                        })
                        .OrderBy(d => d.Name)
                        .ToListAsync();
        }

        public async Task<Director?> GetByIdAsync(int id)
        {
            return await _context.Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(DirectorFormViewModel model)
        {
            var director = new Director
            {
                Name = model.Name,
                Biography = model.Biography
            };

            await _context.Directors.AddAsync(director);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Directors.AnyAsync(d => d.Id == id);
        }

        public async Task<DirectorFormViewModel?> GetForEditAsync(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director == null) return null;

            return new DirectorFormViewModel
            {
                Id = director.Id,
                Name = director.Name,
                Biography = director.Biography
            };
        }

        public async Task EditAsync(DirectorFormViewModel model)
        {
            var director = await _context.Directors.FindAsync(model.Id);
            if (director == null) return;

            director.Name = model.Name;
            director.Biography = model.Biography;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director == null) return false;

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
