using BBLift.Application.Common;
using BBLift.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BBLift.WebApi;

public class DbSeeder
{
    private static readonly string[] RoleNames = [Roles.SuperAdmin, Roles.Admin, Roles.User];

    public static async Task SeedData(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        try
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            foreach (var roleName in RoleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;

                var roleResult = await roleManager.CreateAsync(new AppRole
                {
                    Name = roleName,
                    CreatedAt = TimeProvider.System.GetUtcNow()
                });

                if (!roleResult.Succeeded)
                {
                    Console.WriteLine(string.Join(",", roleResult.Errors.Select(e => e.Description)));
                    return;
                }
            }

            const string superAdminEmail = "super.admin@bblift.com";
            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdmin is null)
            {
                superAdmin = new AppUser
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "super.admin",
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.CreateVersion7().ToString()
                };

                var password = configuration["Seed:SuperAdminPassword"] ?? "Admin@123";

                var userResult = await userManager.CreateAsync(superAdmin, password);
                if (!userResult.Succeeded)
                {
                    Console.WriteLine(string.Join(",", userResult.Errors.Select(e => e.Description)));
                    return;
                }
            }

            if (!await userManager.IsInRoleAsync(superAdmin, Roles.SuperAdmin))
            {
                var roleResult = await userManager.AddToRoleAsync(superAdmin, Roles.SuperAdmin);
                if (!roleResult.Succeeded)
                {
                    Console.WriteLine(string.Join(",", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}