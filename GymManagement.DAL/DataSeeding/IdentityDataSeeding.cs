using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger logger,
            CancellationToken ct = default
            )
        {
            try
            {
                bool hasRoles = await roleManager.Roles.AnyAsync(ct);
                if (!hasRoles)
                {
                    List<IdentityRole> roles = new List<IdentityRole>()
                    {
                        new IdentityRole("SuperAdmin"),
                        new IdentityRole("Admin"),
                    };

                    foreach (IdentityRole role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name))
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }
                }

                bool hasUsers = await userManager.Users.AnyAsync(ct);
                if (!hasUsers)
                {
                    ApplicationUser superAdmin = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Khaled",
                        UserName = "ahmedkhaled",
                        Email = "ahmedkhaled12@gmail.com",
                        PhoneNumber = "01111111111"
                    };
                    await userManager.CreateAsync(superAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");

                    ApplicationUser admin = new ApplicationUser()
                    {
                        FirstName = "mohamed",
                        LastName = "gamal",
                        UserName = "mohamedgamal",
                        Email = "mohamedgamal@gmail.com",
                        PhoneNumber = "01111111111"
                    };
                    await userManager.CreateAsync(admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return;
            }
        }
    }
}