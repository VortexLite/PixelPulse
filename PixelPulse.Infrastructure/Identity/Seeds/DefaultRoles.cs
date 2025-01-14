using Microsoft.AspNetCore.Identity;
using PixelPulse.Domain.Enums;

namespace PixelPulse.Infrastructure.Identity.Seeds;

public class DefaultRoles
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
    }
}