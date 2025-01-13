using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PixelPulse.Infrastructure.Identity.Enums;
using PixelPulse.Infrastructure.Identity.Models;

namespace PixelPulse.Infrastructure.Identity.Seeds;

public class DefaultUsers
{
    public static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        #region SuperAdmin
        var superAdmin = new AppUser()
        {
            UserName = "SA",
            Email = "SAdmin@pixelpulse.com",
            FirstName = "Roman",
            LastName = "Yahodka",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (await userManager.Users.AllAsync(u => u.Id != superAdmin.Id))
        {
            var user = await userManager.FindByEmailAsync(superAdmin.Email);
            if (user == null)
            {
                await userManager.CreateAsync(superAdmin, "12345678");
                await userManager.AddToRoleAsync(superAdmin, Roles.SuperAdmin.ToString());
            }
        }
        #endregion

        #region Admin
        var admin = new AppUser()
        {
            UserName = "Admin",
            Email = "Admin@pixelpulse.com",
            FirstName = "Roman",
            LastName = "Yahodka",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (await userManager.Users.AllAsync(u => u.Id != admin.Id))
        {
            var user = await userManager.FindByEmailAsync(admin.Email);
            if (user == null)
            {
                await userManager.CreateAsync(admin, "12345678");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
        #endregion
        
        #region Employee
        var employee = new AppUser()
        {
            UserName = "Employee",
            Email = "Employee@pixelpulse.com",
            FirstName = "Roman",
            LastName = "Yahodka",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (await userManager.Users.AllAsync(u => u.Id != employee.Id))
        {
            var user = await userManager.FindByEmailAsync(employee.Email);
            if (user == null)
            {
                await userManager.CreateAsync(employee, "12345678");
                await userManager.AddToRoleAsync(employee, Roles.Employee.ToString());
            }
        }
        #endregion
        
        #region Client
        var client = new AppUser()
        {
            UserName = "Client",
            Email = "Client@pixelpulse.com",
            FirstName = "Roman",
            LastName = "Yahodka",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (await userManager.Users.AllAsync(u => u.Id != client.Id))
        {
            var user = await userManager.FindByEmailAsync(client.Email);
            if (user == null)
            {
                await userManager.CreateAsync(client, "12345678");
                await userManager.AddToRoleAsync(client, Roles.Client.ToString());
            }
        }
        #endregion
    }
}