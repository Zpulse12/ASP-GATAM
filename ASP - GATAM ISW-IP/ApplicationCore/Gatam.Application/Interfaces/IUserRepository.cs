using Gatam.Application.CQRS.DTOS;
using Gatam.Domain;

namespace Gatam.Application.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserWithModules(string userId);
    Task<List<ApplicationUser>> GetUsersWithModulesAsync();
}
