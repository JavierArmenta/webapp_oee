using Microsoft.AspNetCore.Identity;
using WebApp.Enums;
using WebApp.Models;

namespace WebApp.Services
{
    public static class Seed
    {
        public static void SeedDB(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedGodAdmin(userManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }
        }

        private static void SeedGodAdmin(UserManager<ApplicationUser> userManager)
        {
            // 🔹 Leer desde ENV
            var adminEmail = Environment.GetEnvironmentVariable("SEED_ADMIN_EMAIL");
            var adminPassword = Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD");
            var adminUserName = Environment.GetEnvironmentVariable("SEED_ADMIN_USERNAME");
            var adminFirstName = Environment.GetEnvironmentVariable("SEED_ADMIN_FIRSTNAME");
            var adminLastName = Environment.GetEnvironmentVariable("SEED_ADMIN_LASTNAME");

            // Validación mínima
            if (string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new Exception("Seed admin variables are not defined in environment");
            }

            // Si el usuario no existe
            if (userManager.FindByEmailAsync(adminEmail).Result == null)
            {
                var adminUserInitial = new ApplicationUser
                {
                    UserName = adminUserName ?? adminEmail,
                    Email = adminEmail,
                    FirstName = adminFirstName ?? "Admin",
                    LastName = adminLastName ?? "System",
                    EmailConfirmed = true
                };

                IdentityResult result =
                    userManager.CreateAsync(adminUserInitial, adminPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUserInitial, Roles.SuperAdmin.ToString()).Wait();
                    userManager.AddToRoleAsync(adminUserInitial, Roles.Admin.ToString()).Wait();
                    userManager.AddToRoleAsync(adminUserInitial, Roles.User.ToString()).Wait();
                }
            }
        }
    }
}