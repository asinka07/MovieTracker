using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MovieTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTracker.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            var adminEmail = "admin@test.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                await userManager.CreateAsync(adminUser, "MovieTracker2@26"); 
                await userManager.AddToRoleAsync(adminUser, "Administrator");
            }

            if (!context.Genres.Any())
            {
                context.Genres.AddRange(
                    new Genre { Name = "Action" },
                    new Genre { Name = "Animation" },
                    new Genre { Name = "Documentary" },
                    new Genre { Name = "Romance" },
                    new Genre { Name = "Comedy" },
                    new Genre { Name = "Rom-Com" },
                    new Genre { Name = "Drama" },
                    new Genre { Name = "Sci-Fi" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
