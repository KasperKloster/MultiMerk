using Domain.Entities.Authentication;

namespace Application.Repositories.ApplicationUsers;

public interface IApplicationUserRepository
{
    Task<ApplicationUser?> GetFirstUserByRoleAsync(string role);
}
