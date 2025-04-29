using System;
using Application.Repositories.ApplicationUsers;
using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.ApplicationUsers;

public class ApplicationUserRepository : IApplicationUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetFirstUserByRoleAsync(string role)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(role);
        return usersInRole.FirstOrDefault();
    }
}