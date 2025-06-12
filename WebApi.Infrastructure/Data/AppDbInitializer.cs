using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApi.Domain.Enums;
using WebApi.Infrastructure.Identity;

namespace WebApi.Infrastructure.Data
{
    public class AppDbInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            // 1. Sinkronisasi semua Role dari enum
            foreach (UserRole role in Enum.GetValues<UserRole>())
            {
                var roleName = role.ToString();
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Buat default admin user jika belum ada
            var superUsers = await userManager.GetUsersInRoleAsync(UserRole.SuperAdmin.ToString());
            if (superUsers.Count == 0)
            {
                const string adminEmail = "superadmin@mail.com";
                const string adminPassword = "@SuperAdmin123";

                var superUser = new ApplicationUser
                {
                    FullName = "Super Administrator",
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(superUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superUser, UserRole.SuperAdmin.ToString());
                }
            }
        }
    }
}