using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication2.Data.DbContexts;

namespace WebApplication2.Data.Seed
{
    public static class DbInitializer
    {
        public static void InsertInitialData(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var service = scope.ServiceProvider;

                try
                {
                    var context = service.GetRequiredService<ApplicationDbContext>();
                    var userManager = service.GetRequiredService<UserManager<IdentityUser>>();
                    var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    var isDevelopment = environment == Environments.Development;

                    context.Database.Migrate();

                    RoleSeed.Seed(roleManager);

                    if (isDevelopment)
                    {
                        UserSeed.Seed(userManager);
                    }

                    CatalogosSeed.Seed(context, isDevelopment, userManager);
                }
                catch (Exception)
                {
                    // Do nothing
                }
            }
        }
    }
}
