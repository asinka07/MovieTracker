using Microsoft.EntityFrameworkCore;
using MovieTracker.Models;

namespace MovieTracker.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        { }

        public DbSet<MovieViewModel> Movies { get; set; }
        public DbSet<GenreViewModel> Genres { get; set; }
        public DbSet<ReviewViewModel> Reviews { get; set; }

    }
}
