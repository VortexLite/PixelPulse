using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PixelPulse.Domain.Entities;
using PixelPulse.Domain.Enums;


namespace PixelPulse.Infrastructure.Content.Seeds;

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
        
        #region Customer
        var customer = new AppUser()
        {
            UserName = "Customer",
            Email = "Customer@pixelpulse.com",
            FirstName = "Roman",
            LastName = "Yahodka",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (await userManager.Users.AllAsync(u => u.Id != customer.Id))
        {
            var user = await userManager.FindByEmailAsync(customer.Email);
            if (user == null)
            {
                await userManager.CreateAsync(customer, "12345678");
                await userManager.AddToRoleAsync(customer, Roles.Customer.ToString());
            }
        }
        #endregion
    }
}