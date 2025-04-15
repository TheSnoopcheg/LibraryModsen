using LibraryModsen.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryModsen;

public static class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using(var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
            var manager = scope.ServiceProvider.GetRequiredService<UserManager<Domain.Models.User>>();
            context.Database.Migrate();

            var user = new Domain.Models.User
            {
                UserName = "admin",
                Email = "admin@admin",
            };

            var createUser = manager.CreateAsync(user, "P@ssw0rd").Result;
            var roleRes = manager.AddToRoleAsync(user, "Admin").Result;
        }
    }
}
