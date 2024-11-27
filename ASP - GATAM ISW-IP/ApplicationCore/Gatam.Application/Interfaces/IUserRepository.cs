using Gatam.Domain;

namespace Gatam.Application.Interfaces;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetUserWithModules(string userId);
    Task<List<ApplicationUser>> GetUsersWithModulesAsync();
    Task<List<ApplicationUser>> GetUsersForBegeleiderAsync(string begeleiderId);
    void RemoveUserRole(UserRole userRole);
    Task<ApplicationUser?> GetUserWithRoles(string userId);


}
