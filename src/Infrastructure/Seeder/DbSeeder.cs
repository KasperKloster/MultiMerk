using Domain.Constants;
using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seeder;

public class DbSeeder
{
    public static async Task SeedData(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

        try
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var rolesAndUsers = new Dictionary<string, (string Username, string Email, string Password)>
            {
                { Roles.Admin,             ("Admin", "admin@gmail.com", "Admin@123") },
                { Roles.User,              ("User", "user@gmail.com", "User@123") },
                { Roles.Freelancer,        ("Freelancer", "freelancer@gmail.com", "Freelancer@123") },
                { Roles.Writer,            ("Writer", "writer@gmail.com", "Writer@123") },
                { Roles.WarehouseWorker,   ("WarehouseWorker","worker@gmail.com", "Worker@123") },
                { Roles.WarehouseManager,  ("WarehouseManager","manager@gmail.com", "Manager@123") },
            };

            // Step 1: Seed Roles
            foreach (var role in rolesAndUsers.Keys)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (result.Succeeded)
                        logger.LogInformation($"Role '{role}' created.");
                    else
                        logger.LogError($"Failed to create role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            // Step 2: Seed Users
            int i = 1;
            foreach (var (role, (username, email, password)) in rolesAndUsers)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser
                    {                         
                        Id = $"00000000-0000-0000-0000-00000000000{i++}",                       
                        Name = role + " User",
                        UserName = username,
                        Email = email,
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var createUserResult = await userManager.CreateAsync(user, password);
                    if (!createUserResult.Succeeded)
                    {
                        logger.LogError($"Failed to create user for role '{role}': {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                        continue;
                    }

                    var addRoleResult = await userManager.AddToRoleAsync(user, role);
                    if (!addRoleResult.Succeeded)
                    {
                        logger.LogError($"Failed to assign role '{role}' to user '{email}': {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
                        continue;
                    }

                    logger.LogInformation($"User '{email}' created and assigned to role '{role}'.");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical($"Seeding failed: {ex.Message}");
        }
    }
}