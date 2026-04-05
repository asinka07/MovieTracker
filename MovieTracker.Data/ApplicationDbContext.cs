using Microsoft.EntityFrameworkCore;
using MovieTracker.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MovieTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Watchlist>().HasKey(w => new { w.UserId, w.MovieId });
        }

    }
}