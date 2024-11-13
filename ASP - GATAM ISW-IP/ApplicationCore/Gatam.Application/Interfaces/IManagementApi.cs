using Gatam.Application.CQRS;
using Gatam.Domain;

namespace Gatam.Application.Interfaces;

public interface IManagementApi
{
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<bool> DeleteUserAsync(string userId);
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user);


    Task<UserDTO> UpdateUserAsync(UserDTO user);
    Task<UserDTO> UpdateUserNicknameAsync(UserDTO user);
    Task<UserDTO> UpdateUserEmailAsync(UserDTO user);
    Task<UserDTO> UpdateUserStatusAsync(string userId, bool isActive);
    Task<UserDTO> UpdateUserRoleAsync(UserDTO user);
    Task<IEnumerable<string>> GetRolesByUserId(string userId);
    Task<UserDTO> GetUserByIdAsync(string userId);
    
}