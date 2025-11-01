using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApplication2.Configuration.Constants;

namespace WebApplication2.Data.Seed
{
    public static class UserSeed
    {
        public static void Seed(UserManager<IdentityUser> userManager)
        {
            userManager.InsertUser("admin@usag.com", "Admin123", Rol.ADMIN);
            userManager.InsertUser("director@usag.com", "Director123", Rol.DIRECTOR);
            userManager.InsertUser("control@usag.com", "Control123", Rol.CONTROL_ESCOLAR);
        }

        private static void InsertUser(this UserManager<IdentityUser> userManager, string email, string password, string rol)
        {
            if (userManager.FindByEmailAsync(email).Result == null)
            {
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email
                };

                var result = userManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, rol)).Wait();
                    userManager.AddToRoleAsync(user, rol).Wait();
                }
            }
        }
    }
}
