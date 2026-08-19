using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Infrastructure.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider)
    {
        var roleManager =
            serviceProvider.GetRequiredService<
                RoleManager<IdentityRole>>();

        var userManager =
            serviceProvider.GetRequiredService<
                UserManager<ApplicationUser>>();

        var context =
            serviceProvider.GetRequiredService<
                ApplicationDbContext>();

        await SeedRolesAsync(roleManager);

        await SeedAdminUserAsync(
            userManager,
            roleManager);

        await SeedDepartmentsAsync(context);

        await SeedDesignationsAsync(context);
    }

    private static async Task SeedRolesAsync(
        RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        {
            "ADMIN",
            "HR",
            "EMPLOYEE"           

        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }
    }

    private static async Task SeedAdminUserAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        const string email = "admin@employee.com";
        const string password = "Admin@123";

        var existingUser =
            await userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            return;
        }

        var admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,

            FirstName = "System",
            LastName = "Administrator"
        };

        var result =
            await userManager.CreateAsync(
                admin,
                password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(x => x.Description));

            throw new Exception(
                $"Failed to create admin user: {errors}");
        }

        await userManager.AddToRoleAsync(
            admin,
            "ADMIN");
    }

    private static async Task SeedDepartmentsAsync(
        ApplicationDbContext context)
    {
        if (await context.Departments.AnyAsync())
        {
            return;
        }

        var departments = new[]
        {
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "IT",
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },

            new Department
            {
                Id = Guid.NewGuid(),
                Name = "HR",
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },

            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Finance",
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }
        };

        await context.Departments.AddRangeAsync(
            departments);

        await context.SaveChangesAsync();
    }

    private static async Task SeedDesignationsAsync(
        ApplicationDbContext context)
    {
        if (await context.Designations.AnyAsync())
        {
            return;
        }

        var designations = new[]
        {
            new Designation
            {
                Id = Guid.NewGuid(),
                Name = "Software Developer",
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },

            new Designation
            {
                Id = Guid.NewGuid(),
                Name = "Senior Software Developer",
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },

            new Designation
            {
                Id = Guid.NewGuid(),
                Name = "HR Manager",
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }
        };

        await context.Designations.AddRangeAsync(
            designations);

        await context.SaveChangesAsync();
    }
}